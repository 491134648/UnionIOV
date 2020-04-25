using Microsoft.Extensions.Logging;
using System.IO.Pipelines;

namespace SuperSocket.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelOptions
    {
        // 4M by default
        public int MaxPackageLength { get; set; } = 1024 * 1024 * 4;
        
        // 4k by default
        public int ReceiveBufferSize { get; set; } = 1024 * 4;
        // 4k by default
        public int SendBufferSize { get; set; } = 1024 * 4;
        /// <summary>
        /// in milliseconds
        /// </summary>
        /// <value></value>
        public int ReceiveTimeout { get; set; }

        /// <summary>
        /// in milliseconds
        /// </summary>
        /// <value></value>
        public int SendTimeout { get; set; }

        public ILogger Logger { get; set; }

        public Pipe In { get; set; }

        public Pipe Out { get; set; }
    }
}
