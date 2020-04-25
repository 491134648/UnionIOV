using System.Threading.Tasks;

namespace Union.Gateway.Abstractions
{
    public interface IUnionMsgProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalNo">设备终端号</param>
        /// <param name="data">808 hex data</param>
        ValueTask ProduceAsync(string terminalNo, byte[] data);
    }
}
