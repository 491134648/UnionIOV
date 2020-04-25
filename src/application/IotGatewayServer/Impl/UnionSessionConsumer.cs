using IotGatewayServer.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Union.Gateway.Abstractions;

namespace IotGatewayServer.Impl
{
    public class UnionSessionConsumer : IUnionSessionConsumer
    {
        public CancellationTokenSource Cts => new CancellationTokenSource();

        private readonly ILogger logger;

        public string TopicName { get; } = UnionGatewayConstants.SessionTopic;

        private readonly UnionSessionService JT808SessionService;
        public UnionSessionConsumer(
            UnionSessionService jT808SessionService,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("UnionSessionConsumer");
            JT808SessionService = jT808SessionService;
        }

        public void OnMessage(Action<(string Notice, string TerminalNo)> callback)
        {
            Task.Run(async () =>
            {
                while (!Cts.IsCancellationRequested)
                {
                    try
                    {
                        var item = await JT808SessionService.ReadAsync(Cts.Token);
                        callback(item);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "");
                    }
                }
            }, Cts.Token);
        }

        public void Unsubscribe()
        {
            Cts.Cancel();
        }

        public void Dispose()
        {
            Cts.Dispose();
        }

        public void Subscribe()
        {

        }
    }
}
