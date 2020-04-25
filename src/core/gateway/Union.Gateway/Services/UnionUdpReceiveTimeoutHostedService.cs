using Union.Gateway.Configurations;
using Union.Gateway.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Union.Gateway.Services
{
    internal class UnionUdpReceiveTimeoutHostedService : BackgroundService
    {
        private readonly ILogger Logger;

        private readonly UnionSessionManager SessionManager;

        private readonly UnionConfiguration Configuration;
        public UnionUdpReceiveTimeoutHostedService(
                IOptions<UnionConfiguration> jT808ConfigurationAccessor,
                ILoggerFactory loggerFactory,
                UnionSessionManager jT808SessionManager
            )
        {
            SessionManager = jT808SessionManager;
            Logger = loggerFactory.CreateLogger("JT808UdpReceiveTimeout");
            Configuration = jT808ConfigurationAccessor.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    List<string> sessionIds = new List<string>();
                    foreach (var item in SessionManager.GetUdpAll())
                    {
                        if (item.ActiveTime.AddSeconds(Configuration.UdpReaderIdleTimeSeconds) < DateTime.Now)
                        {
                            sessionIds.Add(item.SessionID);
                        }
                    }
                    foreach(var item in sessionIds)
                    {
                        SessionManager.RemoveBySessionId(item);
                    }
                    Logger.LogInformation($"[Check Receive Timeout]");
                    Logger.LogInformation($"[Session Online Count]:{SessionManager.UdpSessionCount}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Timeout]");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(Configuration.UdpReceiveTimeoutCheckTimeSeconds), stoppingToken);
                }
            }
        }
    }
}
