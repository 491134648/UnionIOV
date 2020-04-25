using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using Union.Gateway.MsgLogging;

namespace IotGatewayServer.Impl
{
    public class UnionMsgLogging : IUnionMsgLogging
    {
        private readonly ILogger Logger;
        public UnionMsgLogging(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("UnionMsgLogging");
        }
        public void Processor((string TerminalNo, byte[] Data) parameter, UnionMsgLoggingType jT808MsgLoggingType)
        {
            Logger.LogDebug($"{jT808MsgLoggingType.ToString()}-{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
        }
    }
}
