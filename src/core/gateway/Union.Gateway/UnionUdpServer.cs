using System;
using System.Buffers;
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
    public class UnionUdpServer : IHostedService
    {
        private readonly Socket server;

        private readonly ILogger Logger;

        private readonly UnionSessionManager SessionManager;

        private readonly IUnionMsgProducer MsgProducer;

        private readonly JT808Serializer Serializer;

        private readonly UnionAtomicCounterService AtomicCounterService;

        private readonly UnionConfiguration Configuration;

        private readonly IPEndPoint LocalIPEndPoint;

        private readonly UnionNormalReplyMessageHandler JT808NormalReplyMessageHandler;

        public UnionUdpServer(
            IOptions<UnionConfiguration> jT808ConfigurationAccessor,
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory,
            UnionSessionManager jT808SessionManager,
            UnionNormalReplyMessageHandler replyMessageHandler,
            UnionAtomicCounterServiceFactory jT808AtomicCounterServiceFactory)
        {
            SessionManager = jT808SessionManager;
            Logger = loggerFactory.CreateLogger("JT808UdpServer");
            Serializer = jT808Config.GetSerializer();
            JT808NormalReplyMessageHandler = replyMessageHandler;
            AtomicCounterService = jT808AtomicCounterServiceFactory.Create(TransportProtocolType.Udp);
            Configuration = jT808ConfigurationAccessor.Value;
            LocalIPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, Configuration.UdpPort);
            server = new Socket(LocalIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(LocalIPEndPoint);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"JT808 Udp Server start at {IPAddress.Any}:{Configuration.UdpPort}.");
            Task.Run(async() => {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = ArrayPool<byte>.Shared.Rent(Configuration.MiniNumBufferSize);
                    try
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        SocketReceiveMessageFromResult result = await server.ReceiveMessageFromAsync(segment, SocketFlags.None, LocalIPEndPoint);
                        ReaderBuffer(buffer.AsSpan(0, result.ReceivedBytes), server, result);
                    }
                    catch(AggregateException ex)
                    {
                        Logger.LogError(ex, "Receive MessageFrom Async");
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Received Bytes");
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }
        private void ReaderBuffer(ReadOnlySpan<byte> buffer, Socket socket,SocketReceiveMessageFromResult receiveMessageFromResult)
        {
            try
            {
                var package = Serializer.HeaderDeserialize(buffer, minBufferSize: 10240);
                AtomicCounterService.MsgSuccessIncrement();
                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{AtomicCounterService.MsgSuccessCount}");
                if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {receiveMessageFromResult.RemoteEndPoint}]:{package.OriginalData.ToArray().ToHexString()}");
                var session = SessionManager.TryLink(package.Header.TerminalPhoneNo, socket, receiveMessageFromResult.RemoteEndPoint);
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation($"[Connected]:{receiveMessageFromResult.RemoteEndPoint}");
                }
                JT808NormalReplyMessageHandler.Processor(package, session);
            }
            catch (NotImplementedException ex)
            {
                Logger.LogError(ex.Message);
            }
            catch (JT808Exception ex)
            {
                AtomicCounterService.MsgFailIncrement();
                if (Logger.IsEnabled(LogLevel.Information)) Logger.LogInformation($"[Atomic Fail Counter]:{AtomicCounterService.MsgFailCount}");
                Logger.LogError($"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode},[ReaderBuffer]:{buffer.ToArray().ToHexString()}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Fail Counter]:{AtomicCounterService.MsgFailCount}");
                Logger.LogError(ex, $"[ReaderBuffer]:{ buffer.ToArray().ToHexString()}");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("808 Udp Server Stop");
            if (server?.Connected ?? false)
                server.Shutdown(SocketShutdown.Both);
            server?.Close();
            return Task.CompletedTask;
        }
    }
}
