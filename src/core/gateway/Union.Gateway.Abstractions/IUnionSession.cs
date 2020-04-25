using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Union.Gateway.Abstractions.Enums;

namespace Union.Gateway.Abstractions
{
   
    public interface IUnionSession
    {
        /// <summary>
        /// 终端手机号
        /// </summary>
        string TerminalPhoneNo { get; set; }
        string SessionID { get; }
        Socket Client { get; set; }
        DateTime StartTime { get; set; }
        DateTime ActiveTime { get; set; }
        TransportProtocolType TransportProtocolType { get; }
        CancellationTokenSource ReceiveTimeout { get; set; }
        EndPoint RemoteEndPoint { get; set; }
        void Close();
    }
}
