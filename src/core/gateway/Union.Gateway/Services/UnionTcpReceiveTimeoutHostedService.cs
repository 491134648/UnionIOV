using Union.Gateway.Configurations;
using Union.Gateway.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Union.Gateway.Services
{
    internal class UnionTcpReceiveTimeoutHostedService : BackgroundService
    {
        private readonly ILogger Logger;

        private readonly UnionSessionManager SessionManager;

        private readonly UnionConfiguration Configuration;
        public UnionTcpReceiveTimeoutHostedService(
                IOptions<UnionConfiguration> jT808ConfigurationAccessor,
                ILoggerFactory loggerFactory,
                UnionSessionManager jT808SessionManager
            )
        {
            SessionManager = jT808SessionManager;
            Logger = loggerFactory.CreateLogger("JT808TcpReceiveTimeout");
            Configuration = jT808ConfigurationAccessor.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var item in SessionManager.GetTcpAll())
                    {
                        if (item.ActiveTime.AddSeconds(Configuration.TcpReaderIdleTimeSeconds) < DateTime.Now)
                        {
                            item.ReceiveTimeout.Cancel();
                        }
                    }
                    Logger.LogInformation($"[Check Receive Timeout]");
                    Logger.LogInformation($"[Session Online Count]:{SessionManager.TcpSessionCount}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Timeout]");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(Configuration.TcpReceiveTimeoutCheckTimeSeconds), stoppingToken);
                }
            }
        }
    }
}
