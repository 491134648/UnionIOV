using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using Union.Gateway.MsgIdHandler;

namespace IotGatewayServer.Impl
{
    public class UnionMsgIdHandler : IUnionMsgIdHandler
    {
        private readonly ILogger Logger;
        public UnionMsgIdHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("UnionMsgIdHandler");
        }
        public void Processor((string TerminalNo, byte[] Data) parameter)
        {
            Logger.LogDebug($"{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
        }
    }
}
