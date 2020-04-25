using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Union.Gateway.Abstractions;
using Union.Gateway.Transmit.Configs;

namespace Union.Gateway.Transmit
{
    public static  class UnionTransmitExtensions
    {
        /// <summary>
        /// 转发服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IUnionClientBuilder AddTransmit(this IUnionClientBuilder jT808ClientBuilder,IConfiguration configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<UnionTransmitService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionTransmitHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 转发服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808NormalGatewayBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IUnionNormalGatewayBuilder AddTransmit(this IUnionNormalGatewayBuilder jT808NormalGatewayBuilder, IConfiguration configuration)
        {
            jT808NormalGatewayBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808NormalGatewayBuilder.JT808Builder.Services.AddSingleton<UnionTransmitService>();
            return jT808NormalGatewayBuilder;
        }
    }
}
