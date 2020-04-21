﻿using JT808.Protocol.Formatters;
using JT808.Protocol.MessagePack;
using System.Collections.Generic;

namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 删除路线
    /// 0x8607
    /// </summary>
    public class JT808_0x8607 : JT808Bodies, IJT808MessagePackFormatter<JT808_0x8607>
    {
        public override ushort MsgId { get; } = 0x8607;
        /// <summary>
        /// 区域数
        /// 本条消息中包含的区域数，不超过 125 个，多于 125个建议用多条消息，0 为删除所有圆形区域
        /// </summary>
        public byte AreaCount { get; set; }
        /// <summary>
        /// 区域ID集合
        /// </summary>
        public List<uint> AreaIds { get; set; }

        public JT808_0x8607 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8607 jT808_0X8607 = new JT808_0x8607();
            jT808_0X8607.AreaCount = reader.ReadByte();
            jT808_0X8607.AreaIds = new List<uint>();
            for (var i = 0; i < jT808_0X8607.AreaCount; i++)
            {
                jT808_0X8607.AreaIds.Add(reader.ReadUInt32());
            }
            return jT808_0X8607;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8607 value, IJT808Config config)
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
