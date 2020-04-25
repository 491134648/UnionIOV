using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Union.Gateway.Abstractions;

namespace Union.Gateway.Traffic
{
    public static class UnionTrafficServiceExtensions
    {
        /// <summary>
        /// 消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IUnionClientBuilder AddTraffic<TIJT808Traffic>(this IUnionClientBuilder jT808ClientBuilder)
            where TIJT808Traffic : IUnionTraffic
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionTraffic), typeof(TIJT808Traffic));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionTrafficServiceHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IUnionClientBuilder AddTraffic(this IUnionClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionTraffic), typeof(UnionTrafficDefault));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionTrafficServiceHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="unionNormalGatewayBuilder"></param>
        /// <returns></returns>
        public static IUnionNormalGatewayBuilder AddTraffic<TIJT808Traffic>(this IUnionNormalGatewayBuilder unionNormalGatewayBuilder)
            where TIJT808Traffic : IUnionTraffic
        {
            unionNormalGatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionTraffic), typeof(TIJT808Traffic));
            return unionNormalGatewayBuilder;
        }


        /// <summary>
        /// 消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="unionNormalGatewayBuilder"></param>
        /// <returns></returns>
        public static IUnionNormalGatewayBuilder AddTraffic(this IUnionNormalGatewayBuilder unionNormalGatewayBuilder)
        {
            unionNormalGatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionTraffic), typeof(UnionTrafficDefault));
            return unionNormalGatewayBuilder;
        }
    }
}
