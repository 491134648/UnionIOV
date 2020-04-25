using Union.Gateway.Abstractions;
using Union.Gateway.Configurations;
using Union.Gateway.Internal;
using Union.Gateway.Services;
using Union.Gateway.Session;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Union.Gateway.TestHosting")]
[assembly: InternalsVisibleTo("Union.Gateway.Test")]
namespace Union.Gateway
{
    public static partial class UnionGatewayExtensions
    {
        public static IUnionNormalGatewayBuilder AddNormalGateway(this IJT808Builder jT808Builder, Action<UnionConfiguration> config)
        {
            IUnionNormalGatewayBuilder server = new UnionNormalGatewayBuilderDefault(jT808Builder);
            server.JT808Builder.Services.AddSingleton<UnionNormalReplyMessageHandler>();
            server.JT808Builder.Services.Configure(config);
            server.AddJT808Core();
            return server;
        }
        public static IUnionNormalGatewayBuilder AddNormalGateway(this IJT808Builder jT808Builder, IConfiguration configuration)
        {
            IUnionNormalGatewayBuilder server = new UnionNormalGatewayBuilderDefault(jT808Builder);
            server.JT808Builder.Services.AddSingleton<UnionNormalReplyMessageHandler>();
            server.JT808Builder.Services.Configure<UnionConfiguration>(configuration.GetSection("JT808Configuration"));
            server.AddJT808Core();
            return server;
        }

        public static IUnionNormalGatewayBuilder ReplaceNormalReplyMessageHandler<TJT808NormalReplyMessageHandler>(this IUnionNormalGatewayBuilder config)
            where TJT808NormalReplyMessageHandler : UnionNormalReplyMessageHandler
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(UnionNormalReplyMessageHandler),typeof(TJT808NormalReplyMessageHandler), ServiceLifetime.Singleton));
            return config;
        }

        public static IUnionGatewayBuilder AddTcp(this IUnionGatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<UnionTcpServer>();
            config.JT808Builder.Services.AddHostedService<UnionTcpReceiveTimeoutHostedService>();
            return config;
        }

        public static IUnionGatewayBuilder AddUdp(this IUnionGatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<UnionUdpServer>();
            config.JT808Builder.Services.AddHostedService<UnionUdpReceiveTimeoutHostedService>();
            return config;
        }

        public static IUnionGatewayBuilder AddGrpc(this IUnionGatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<UnionGrpcServer>();
            return config;
        }
        private static IUnionGatewayBuilder AddJT808Core(this IUnionGatewayBuilder config)
        {
            config.JT808Builder.Services.TryAddSingleton<UnionAtomicCounterServiceFactory>();
            config.JT808Builder.Services.TryAddSingleton<UnionSessionManager>();
            return config;
        }
    }
}