using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Union.Gateway.Abstractions;

namespace Union.Gateway.SessionNotice
{
    public class UnionSessionNoticeHostedService : IHostedService
    {
        private readonly UnionSessionNoticeService jT808SessionNoticeService;
        private readonly IUnionSessionConsumer jT808SessionConsumer;
        public UnionSessionNoticeHostedService(
            IUnionSessionConsumer jT808SessionConsumer,
            UnionSessionNoticeService jT808SessionNoticeService)
        {
            this.jT808SessionNoticeService = jT808SessionNoticeService;
            this.jT808SessionConsumer = jT808SessionConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Subscribe();
            jT808SessionConsumer.OnMessage(jT808SessionNoticeService.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
