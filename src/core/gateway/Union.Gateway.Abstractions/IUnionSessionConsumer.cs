using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Union.Gateway.Abstractions
{
    public interface IUnionSessionConsumer : IUnionPubSub, IDisposable
    {
        void OnMessage(Action<(string Notice, string TerminalNo)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
