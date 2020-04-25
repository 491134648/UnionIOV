using Microsoft.Extensions.DependencyInjection;
using Union.Gateway.Abstractions;

namespace Union.Gateway.SessionNotice
{
    public static class UnionSessionNoticeExtensions
    {
        /// <summary>
        /// 会话通知服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IUnionClientBuilder AddSessionNotice(this IUnionClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<UnionSessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionSessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 消息会话通知服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IUnionClientBuilder AddSessionNotice<TSessionNoticeService>(this IUnionClientBuilder jT808ClientBuilder)
           where TSessionNoticeService : UnionSessionNoticeService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<UnionSessionNoticeService, TSessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionSessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 会话通知服务（不同的消费者实例）
        /// </summary>
        /// <param name="unionNormalGatewayBuilder"></param>
        /// <returns></returns>
        public static IUnionNormalGatewayBuilder AddSessionNotice(this IUnionNormalGatewayBuilder unionNormalGatewayBuilder)
        {
            unionNormalGatewayBuilder.JT808Builder.Services.AddSingleton<UnionSessionNoticeService>();
            unionNormalGatewayBuilder.JT808Builder.Services.AddHostedService<UnionSessionNoticeHostedService>();
            return unionNormalGatewayBuilder;
        }

        /// <summary>
        /// 消息会话通知服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="unionNormalGatewayBuilder"></param>
        /// <returns></returns>
        public static IUnionNormalGatewayBuilder AddSessionNotice<TSessionNoticeService>(this IUnionNormalGatewayBuilder unionNormalGatewayBuilder)
           where TSessionNoticeService : UnionSessionNoticeService
        {
            unionNormalGatewayBuilder.JT808Builder.Services.AddSingleton<UnionSessionNoticeService, TSessionNoticeService>();
            unionNormalGatewayBuilder.JT808Builder.Services.AddHostedService<UnionSessionNoticeHostedService>();
            return unionNormalGatewayBuilder;
        }
    }
}
