using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Union.Gateway.Abstractions
{
    public interface IUnionSessionProducer : IUnionPubSub, IDisposable
    {
        ValueTask ProduceAsync(string notice, string terminalNo);
    }
}
