using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Union.Gateway.Abstractions;

namespace Union.Gateway.Traffic
{
    public class UnionTrafficServiceHostedService : IHostedService
    {
        private readonly IUnionMsgConsumer jT808MsgConsumer;
        private readonly IUnionTraffic jT808Traffic;

        public UnionTrafficServiceHostedService(
            IUnionTraffic jT808Traffic,
            IUnionMsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808Traffic = jT808Traffic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((item) => {
                //string str = item.Data.ToHexString();
                jT808Traffic.Increment(item.TerminalNo, DateTime.Now.ToString("yyyyMMdd"), item.Data.Length);
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
