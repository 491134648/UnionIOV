using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Union.Gateway.Abstractions;

namespace Union.Gateway.MsgIdHandler
{
    public class UnionMsgIdHandlerHostedService : IHostedService
    {
        private readonly IUnionMsgConsumer jT808MsgConsumer;

        private readonly IUnionMsgIdHandler jT808MsgIdHandler;
        public UnionMsgIdHandlerHostedService(
            IUnionMsgIdHandler jT808MsgIdHandler,
            IUnionMsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgIdHandler = jT808MsgIdHandler;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808MsgIdHandler.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
