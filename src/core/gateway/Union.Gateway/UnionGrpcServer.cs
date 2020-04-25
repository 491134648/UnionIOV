using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Union.Gateway.Configurations;
using Union.Gateway.GrpcService;
using Union.Gateway.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Union.Gateway
{
    public class UnionGrpcServer : IHostedService
    {
        private readonly ILogger Logger;
        private readonly UnionConfiguration Configuration;
        private readonly IServiceProvider ServiceProvider;
        private Server server;
        public UnionGrpcServer(
                IServiceProvider serviceProvider,
                IOptions<UnionConfiguration> jT808ConfigurationAccessor,
                ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("JT808GrpcServer");
            Configuration = jT808ConfigurationAccessor.Value;
            ServiceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            server = new Server
            {
                Services = { UnionGateway.BindService(new UnionGatewayService(ServiceProvider)) },
                Ports = { new ServerPort(Configuration.WebApiHost, Configuration.WebApiPort, ServerCredentials.Insecure) }
            };
            Logger.LogInformation($"JT808 Grpc Server start at {Configuration.WebApiHost}:{Configuration.WebApiPort}.");
            try
            {
                server.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "JT808 Grpc Server start error");
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("JT808 Grpc Server Stop");
            server.ShutdownAsync();
            return Task.CompletedTask;
        }
    }
}
