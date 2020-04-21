﻿using JT808.Protocol.Formatters;
using JT808.Protocol.MessagePack;
using System.Collections.Generic;

namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 删除矩形区域
    /// 0x8603
    /// </summary>
    public class JT808_0x8603 : JT808Bodies, IJT808MessagePackFormatter<JT808_0x8603>
    {
        public override ushort MsgId { get; } = 0x8603;
        /// <summary>
        /// 区域数
        /// 本条消息中包含的区域数，不超过 125 个，多于 125个建议用多条消息，0 为删除所有圆形区域
        /// </summary>
        public byte AreaCount { get; set; }
        /// <summary>
        /// 区域ID集合
        /// </summary>
        public List<uint> AreaIds { get; set; }

        public JT808_0x8603 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8603 jT808_0X8603 = new JT808_0x8603();
            jT808_0X8603.AreaCount = reader.ReadByte();
            jT808_0X8603.AreaIds = new List<uint>();
            for (var i = 0; i < jT808_0X8603.AreaCount; i++)
            {
                jT808_0X8603.AreaIds.Add(reader.ReadUInt32());
            }
            return jT808_0X8603;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8603 value, IJT808Config config)
        {
            if (value.AreaIds != null)
            {
                writer.WriteByte((byte)value.AreaIds.Count);
                foreach (var item in value.AreaIds)
                {
                    writer.WriteUInt32(item);
                }
            }
        }
    }
}
