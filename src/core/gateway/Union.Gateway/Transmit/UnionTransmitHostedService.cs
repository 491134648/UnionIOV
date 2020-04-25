using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Union.Gateway.Abstractions;

namespace Union.Gateway.Transmit
{
    public class UnionTransmitHostedService:IHostedService
    {
        private readonly UnionTransmitService jT808TransmitService;
        private readonly IUnionMsgConsumer jT808MsgConsumer;
        public UnionTransmitHostedService(
            IUnionMsgConsumer jT808MsgConsumer,
            UnionTransmitService jT808TransmitService)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808TransmitService.SendAsync);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            jT808TransmitService.Stop();
            return Task.CompletedTask;
        }
    }
}
