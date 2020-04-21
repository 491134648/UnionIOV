﻿namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 查询终端属性
    /// </summary>
    public class JT808_0x8107 : JT808Bodies
    {
        public override ushort MsgId { get; } = 0x8107;
        /// <summary>
        /// 跳过数据体序列化
        /// </summary>
        public override bool SkipSerialization { get; set; } = true;
    }
}
