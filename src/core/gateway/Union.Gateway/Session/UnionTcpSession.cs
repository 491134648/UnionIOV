using Union.Gateway.Abstractions;
using Union.Gateway.Abstractions.Enums;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Union.Gateway.Session
{
    public class UnionTcpSession: IUnionSession
    {
        public UnionTcpSession(Socket client)
        {
            Client = client;
            RemoteEndPoint = client.RemoteEndPoint;
            ActiveTime = DateTime.Now;
            StartTime = DateTime.Now;
            SessionID = Guid.NewGuid().ToString("N");
            ReceiveTimeout = new CancellationTokenSource();
        }

        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }
        public DateTime ActiveTime { get; set; }
        public DateTime StartTime { get; set; }
        public TransportProtocolType TransportProtocolType { get;} = TransportProtocolType.Tcp;
        public string SessionID { get; }
        public Socket Client { get; set; }
        public CancellationTokenSource ReceiveTimeout { get; set; }
        public EndPoint RemoteEndPoint { get ; set; }

        public void Close()
        {
            try
            {
                Client.Shutdown(SocketShutdown.Both);
            }
            catch { }
            finally
            {
                Client.Close();
            }
        }
    }
}
