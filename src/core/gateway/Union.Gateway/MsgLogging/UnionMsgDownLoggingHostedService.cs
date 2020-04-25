using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Union.Gateway.Abstractions;

namespace Union.Gateway.MsgLogging
{
    public class UnionMsgDownLoggingHostedService : IHostedService
    {
        private readonly IUnionMsgReplyConsumer jT808MsgReplyConsumer;
        private readonly IUnionMsgLogging jT808MsgLogging;
        public UnionMsgDownLoggingHostedService(
            IUnionMsgLogging jT808MsgLogging,
            IUnionMsgReplyConsumer jT808MsgReplyConsumer)
        {
            this.jT808MsgReplyConsumer = jT808MsgReplyConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Subscribe();
            jT808MsgReplyConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, UnionMsgLoggingType.down);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
