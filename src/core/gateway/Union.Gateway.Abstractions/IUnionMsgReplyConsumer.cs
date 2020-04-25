using System;
using System.Threading;

namespace Union.Gateway.Abstractions
{
    public interface IUnionMsgReplyConsumer
    {
        void OnMessage(Action<(string TerminalNo, byte[] Data)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
