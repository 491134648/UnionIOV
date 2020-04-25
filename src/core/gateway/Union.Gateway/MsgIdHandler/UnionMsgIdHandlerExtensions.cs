using Microsoft.Extensions.DependencyInjection;
using Union.Gateway.Abstractions;

namespace Union.Gateway.MsgIdHandler
{
    public static class UnionMsgIdHandlerExtensions
    {
        public static IUnionClientBuilder AddMsgIdHandler<TJT808MsgIdHandler>(this IUnionClientBuilder jT808ClientBuilder)
            where TJT808MsgIdHandler: IUnionMsgIdHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IUnionMsgIdHandler),typeof(TJT808MsgIdHandler));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<UnionMsgIdHandlerHostedService>();
            return jT808ClientBuilder;
        }
    }
}
