using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Union.Gateway.Abstractions;
using Union.Gateway.Abstractions.Enums;
using Union.Gateway.Configurations;
using Union.Gateway.Services;
using Union.Gateway.Session;
using JT808.Protocol;
using JT808.Protocol.Exceptions;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Union.Gateway
{
    public class UnionTcpServer : IHostedService
    {
        private Socket server;

        private readonly ILogger Logger;

        private readonly UnionSessionManager SessionManager;

        private readonly IUnionMsgProducer MsgProducer;

        private readonly JT808Serializer Serializer;

        private readonly UnionAtomicCounterService AtomicCounterService;

        private readonly UnionConfiguration Configuration;

        private readonly UnionNormalReplyMessageHandler _unionNormalReplyMessageHandler;
        /// <summary>
        /// 使用正常方式
        /// </summary>
        /// <param name="jT808ConfigurationAccessor"></param>
        /// <param name="jT808Config"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="jT808SessionManager"></param>
        /// <param name="replyMessageHandler"></param>
        /// <param name="jT808AtomicCounterServiceFactory"></param>
        public UnionTcpServer(
                IOptions<UnionConfiguration> jT808ConfigurationAccessor,
                IJT808Config jT808Config,
                ILoggerFactory loggerFactory,
                UnionSessionManager jT808SessionManager,
                UnionNormalReplyMessageHandler replyMessageHandler,
                UnionAtomicCounterServiceFactory jT808AtomicCounterServiceFactory)
        {
            SessionManager = jT808SessionManager;
            Logger = loggerFactory.CreateLogger("JT808TcpServer");
            Serializer = jT808Config.GetSerializer();
            _unionNormalReplyMessageHandler = replyMessageHandler;
            AtomicCounterService = jT808AtomicCounterServiceFactory.Create(TransportProtocolType.Tcp);
            Configuration = jT808ConfigurationAccessor.Value;
            InitServer();
        }

        private void InitServer()
        {
            var IPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, Configuration.TcpPort);
            server = new Socket(IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Configuration.MiniNumBufferSize);
            //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Configuration.MiniNumBufferSize);
            server.LingerState = new LingerOption(false, 0);
            server.Bind(IPEndPoint);
            server.Listen(Configuration.SoBacklog);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"JT808 TCP Server start at {IPAddress.Any}:{Configuration.TcpPort}.");
            Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var socket = await server.AcceptAsync();
                    UnionTcpSession jT808TcpSession = new UnionTcpSession(socket);
                    SessionManager.TryAdd(jT808TcpSession);
                    await Task.Factory.StartNew(async (state) =>
                    {
                        var session = (UnionTcpSession)state;
                        if (Logger.IsEnabled(LogLevel.Information))
                        {
                            Logger.LogInformation($"[Connected]:{session.Client.RemoteEndPoint}");
                        }
                        var pipe = new Pipe();
                        Task writing = FillPipeAsync(session, pipe.Writer);
                        Task reading = ReadPipeAsync(session, pipe.Reader);
                        await Task.WhenAll(reading, writing);
                        SessionManager.RemoveBySessionId(session.SessionID);
                    }, jT808TcpSession);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }
        private async Task FillPipeAsync(UnionTcpSession session, PipeWriter writer)
        {
            while (true)
            {
                try
                {
                    Memory<byte> memory = writer.GetMemory(Configuration.MiniNumBufferSize);
                    //设备多久没发数据就断开连接 Receive Timeout.
                    int bytesRead = await session.Client.ReceiveAsync(memory, SocketFlags.None, session.ReceiveTimeout.Token);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (OperationCanceledException ex)
                {
                    Logger.LogError($"[Receive Timeout]:{session.Client.RemoteEndPoint}");
                    break;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message}]:{session.Client.RemoteEndPoint}");
                    break;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Error]:{session.Client.RemoteEndPoint}");
                    break;
                }
#pragma warning restore CA1031 // Do not catch general exception types
                FlushResult result = await writer.FlushAsync();
                if (result.IsCompleted)
                {
                    break;
                }
            }
            writer.Complete();
        }
        private async Task ReadPipeAsync(UnionTcpSession session, PipeReader reader)
        {
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                if (result.IsCompleted)
                {
                    break;
                }
                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition consumed = buffer.Start;
                SequencePosition examined = buffer.End;
                try
                {
                    if (result.IsCanceled) break;
                    if (buffer.Length > 0)
                    {
                        ReaderBuffer(ref buffer, session, out consumed, out examined);
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[ReadPipe Error]:{session.Client.RemoteEndPoint}");
                    break;
                }
#pragma warning restore CA1031 // Do not catch general exception types
                finally
                {
                    reader.AdvanceTo(consumed, examined);
                }
            }
            reader.Complete();
        }
        private void ReaderBuffer(ref ReadOnlySequence<byte> buffer, UnionTcpSession session, out SequencePosition consumed, out SequencePosition examined)
        {
            consumed = buffer.Start;
            examined = buffer.End;
            SequenceReader<byte> seqReader = new SequenceReader<byte>(buffer);
            if (seqReader.TryPeek(out byte beginMark))
            {
                if (beginMark != JT808Package.BeginFlag) throw new ArgumentException("Not JT808 Packages.");
            }
            byte mark = 0;
            long totalConsumed = 0;
            while (!seqReader.End)
            {
                if (seqReader.IsNext(JT808Package.BeginFlag, advancePast: true))
                {
                    if (mark == 1)
                    {
                        ReadOnlySpan<byte> contentSpan = ReadOnlySpan<byte>.Empty;
                        try
                        {
                            contentSpan = seqReader.Sequence.Slice(totalConsumed, seqReader.Consumed - totalConsumed).FirstSpan;
                            //过滤掉不是808标准包（14）
                            //（头）1+（消息 ID ）2+（消息体属性）2+（终端手机号）6+（消息流水号）2+（检验码 ）1+（尾）1
                            if (contentSpan.Length > 14)
                            {
                                var package = Serializer.HeaderDeserialize(contentSpan, minBufferSize: 10240);
                                AtomicCounterService.MsgSuccessIncrement();
                                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{AtomicCounterService.MsgSuccessCount}");
                                if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {session.Client.RemoteEndPoint}]:{package.OriginalData.ToArray().ToHexString()}");
                                SessionManager.TryLink(package.Header.TerminalPhoneNo, session);
                                _unionNormalReplyMessageHandler.Processor(package, session);
                            }
                        }
                        catch (NotImplementedException ex)
                        {
                            Logger.LogError(ex.Message);
                        }
                        catch (JT808Exception ex)
                        {
                            AtomicCounterService.MsgFailIncrement();
                            if (Logger.IsEnabled(LogLevel.Information)) Logger.LogInformation($"[Atomic Fail Counter]:{AtomicCounterService.MsgFailCount}");
                            Logger.LogError($"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode},[ReaderBuffer]:{contentSpan.ToArray().ToHexString()}");
                        }
                        totalConsumed += (seqReader.Consumed - totalConsumed);
                        if (seqReader.End) break;
                        seqReader.Advance(1);
                        mark = 0;
                    }
                    mark++;
                }
                else
                {
                    seqReader.Advance(1);
                }
            }
            if (seqReader.Length == totalConsumed)
            {
                examined = consumed = buffer.End;
            }
            else
            {
                consumed = buffer.GetPosition(totalConsumed);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("808 Tcp Server Stop");
            if (server?.Connected ?? false)
                server.Shutdown(SocketShutdown.Both);
            server?.Close();
            return Task.CompletedTask;
        }
    }
}
