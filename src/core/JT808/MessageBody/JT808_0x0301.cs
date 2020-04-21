﻿using JT808.Protocol.Formatters;
using JT808.Protocol.Interfaces;
using JT808.Protocol.MessagePack;
using System;

namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 事件报告
    /// 0x0301
    /// </summary>
    [Obsolete("2019版本已作删除")]
    public class JT808_0x0301 : JT808Bodies, IJT808MessagePackFormatter<JT808_0x0301>, IJT808_2019_Version
    {
        public override ushort MsgId { get; } = 0x0301;
        /// <summary>
        /// 事件 ID 
        /// </summary>
        public byte EventId { get; set; }
        public JT808_0x0301 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x0301 jT808_0X0301 = new JT808_0x0301();
            jT808_0X0301.EventId = reader.ReadByte();
            return jT808_0X0301;
        }
        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x0301 value, IJT808Config config)
        {
            writer.WriteByte(value.EventId);
        }
    }
}
