using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IotGatewayServer.Services
{
    public class UnionSessionService
    {
        private readonly Channel<(string Notice, string TerminalNo)> _channel;

        public UnionSessionService()
        {
            _channel = Channel.CreateUnbounded<(string Notice, string TerminalNo)>();
        }

        public async ValueTask WriteAsync(string notice, string terminalNo)
        {
            await _channel.Writer.WriteAsync((notice, terminalNo));
        }
        public async ValueTask<(string Notice, string TerminalNo)> ReadAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
