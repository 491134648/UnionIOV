using System.Net.Sockets;
using Union.Gateway.Abstractions.Enums;

namespace Union.Gateway.Abstractions.Extensions
{
    public static class UnionSessionExtensions
    {
        /// <summary>
        /// 发送字节消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        public static async void SendAsync(this IUnionSession session, byte[] data)
        {
            if (session.TransportProtocolType == TransportProtocolType.Tcp)
            {
                await session.Client.SendAsync(data, SocketFlags.None);
            }
            else
            {
                await session.Client.SendToAsync(data, SocketFlags.None, session.RemoteEndPoint);
            }
        }
    }
}
