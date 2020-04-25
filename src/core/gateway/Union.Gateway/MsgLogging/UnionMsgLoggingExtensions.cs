using Microsoft.Extensions.DependencyInjection;
using Union.Gateway.Abstractions;

namespace Union.Gateway.MsgLogging
{
    public static class UnionMsgLoggingExtensions
    {
        public static IUnionClientBuilder AddMsgLogging<TJT808MsgLogging>(this IUnionClientBuilder jT808ClientBuilder)
            where TJT808MsgLogging: IUnionMsgLogging
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionMsgLogging),typeof(TJT808MsgLogging));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionMsgDownLoggingHostedService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionMsgUpLoggingHostedService>();
            return jT808ClientBuilder;
        }

        public static IUnionNormalGatewayBuilder AddMsgLogging<TJT808MsgLogging>(this IUnionNormalGatewayBuilder unionNormalGatewayBuilder)
            where TJT808MsgLogging : IUnionMsgLogging
        {
            unionNormalGatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionMsgLogging), typeof(TJT808MsgLogging));
            return unionNormalGatewayBuilder;
        }
    }
}
