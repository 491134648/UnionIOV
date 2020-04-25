using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Union.Gateway.Abstractions;

namespace Union.Gateway.MsgLogging
{
    public class UnionMsgUpLoggingHostedService : IHostedService
    {
        private readonly IUnionMsgConsumer jT808MsgConsumer;
        private readonly IUnionMsgLogging jT808MsgLogging;
        public UnionMsgUpLoggingHostedService(
            IUnionMsgLogging jT808MsgLogging,
            IUnionMsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, UnionMsgLoggingType.up);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
