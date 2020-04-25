using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Union.Gateway.Abstractions
{
    public interface IUnionMsgConsumer
    {
        void OnMessage(Action<(string TerminalNo, byte[] Data)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
