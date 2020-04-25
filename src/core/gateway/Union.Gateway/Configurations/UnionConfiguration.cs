namespace Union.Gateway.Configurations
{
    public class UnionConfiguration
    {
        public int TcpPort { get; set; } = 808;
        public int UdpPort { get; set; } = 808;
        public int WebApiPort { get; set; } = 828;
        public string WebApiHost{ get; set; } = "localhost";
        /// <summary>
        /// WebApi 默认token 123456 
        /// </summary>
        public string WebApiToken { get; set; } = "123456";
        public int SoBacklog { get; set; } = 8192;
        /// <summary>
        /// 最小缓冲大小
        /// </summary>
        public int MiniNumBufferSize { get; set; } = 4096;
        /// <summary>
        /// Tcp读超时 
        /// 默认10分钟检查一次
        /// </summary>
        public int TcpReaderIdleTimeSeconds { get; set; } = 60*10;
        /// <summary>
        /// Tcp 60s检查一次
        /// </summary>
        public int TcpReceiveTimeoutCheckTimeSeconds { get; set; } = 60;
        /// <summary>
        /// Udp读超时
        /// </summary>
        public int UdpReaderIdleTimeSeconds { get; set; } = 60;
        /// <summary>
        /// Udp 60s检查一次
        /// </summary>
        public int UdpReceiveTimeoutCheckTimeSeconds { get; set; } = 60;
    }
}
