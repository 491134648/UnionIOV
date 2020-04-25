using IotGatewayServer.Services;
using System.Threading.Tasks;
using Union.Gateway.Abstractions;

namespace IotGatewayServer.Impl
{
    public class UnionSessionProducer : IUnionSessionProducer
    {
        public string TopicName { get; } = UnionGatewayConstants.SessionTopic;

        private readonly UnionSessionService JT808SessionService;

        public UnionSessionProducer(UnionSessionService jT808SessionService)
        {
            JT808SessionService = jT808SessionService;
        }

        public async ValueTask ProduceAsync(string notice,string terminalNo)
        {
            await JT808SessionService.WriteAsync(notice, terminalNo);
        }

        public void Dispose()
        {
        }
    }
}
