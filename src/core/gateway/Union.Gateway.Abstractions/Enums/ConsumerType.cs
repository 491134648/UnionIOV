using System;

namespace Union.Gateway.Abstractions.Enums
{
    /// <summary>
    /// 消费者类型
    /// </summary>
    [Flags]
    public enum ConsumerType : int
    {
        /// <summary>
        /// 消息id
        /// </summary>
        MsgIdHandlerConsumer = 1,
        /// <summary>
        /// 消息日志
        /// </summary>
        MsgLoggingConsumer = 2,
        /// <summary>
        /// 重试消息
        /// </summary>
        ReplyMessageConsumer = 4,
        /// <summary>
        /// 流量
        /// </summary>
        TrafficConsumer = 8,
        /// <summary>
        /// 传输
        /// </summary>
        TransmitConsumer = 16,
        /// <summary>
        /// 重试消息日志
        /// </summary>
        ReplyMessageLoggingConsumer = 32,
        /// <summary>
        /// 所有
        /// </summary>
        All = 64,
    }
}
