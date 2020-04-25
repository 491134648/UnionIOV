using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Union.Gateway.Traffic;

namespace IotGatewayServer.Jobs
{
    public class TrafficJob : IHostedService
    {
        private readonly IUnionTraffic jT808Traffic;
        private readonly ILogger Logger;
        public TrafficJob(
            ILoggerFactory loggerFactory,
            IUnionTraffic jT808Traffic)
        {
            Logger = loggerFactory.CreateLogger("TrafficJob");
            this.jT808Traffic = jT808Traffic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(2 * 1000);
                    foreach (var item in jT808Traffic.GetAll())
                    {
                        Logger.LogDebug($"{item.Item1}-{item.Item2}");
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
