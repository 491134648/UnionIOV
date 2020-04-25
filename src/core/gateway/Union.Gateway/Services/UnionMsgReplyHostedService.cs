using Union.Gateway.Abstractions;
using Union.Gateway.Session;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Union.Gateway.Services
{
    internal class UnionMsgReplyHostedService : IHostedService
    {
        private readonly UnionSessionManager JT808SessionManager;

        private readonly IUnionMsgReplyConsumer JT808MsgReplyConsumer;

        public UnionMsgReplyHostedService(
            IUnionMsgReplyConsumer jT808MsgReplyConsumer,
            UnionSessionManager jT808SessionManager)
        {
            JT808MsgReplyConsumer = jT808MsgReplyConsumer;
            JT808SessionManager = jT808SessionManager;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            JT808MsgReplyConsumer.OnMessage(async(item) =>
            {
                await JT808SessionManager.TrySendByTerminalPhoneNoAsync(item.TerminalNo, item.Data);
            });
            JT808MsgReplyConsumer.Subscribe();
            return Task.CompletedTask;    
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            JT808MsgReplyConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
