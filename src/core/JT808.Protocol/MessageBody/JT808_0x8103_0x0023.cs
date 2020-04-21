﻿using JT808.Protocol.Attributes;
using JT808.Protocol.Formatters;
using JT808.Protocol.Interfaces;
using JT808.Protocol.MessagePack;

namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 从服务器APN。该值为空时，终端应使用主服务器相同配置
    /// 2019版本
    /// </summary>
    public class JT808_0x8103_0x0023 : JT808_0x8103_BodyBase, IJT808MessagePackFormatter<JT808_0x8103_0x0023>, IJT808_2019_Version
    {
        public override uint ParamId { get; set; } = 0x0023;
        /// <summary>
        /// 数据 长度
        /// </summary>
        public override byte ParamLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParamValue { get; set; }
        public JT808_0x8103_0x0023 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8103_0x0023 value = new JT808_0x8103_0x0023();
            value.ParamId = reader.ReadUInt32();
            value.ParamLength = reader.ReadByte();
            value.ParamValue = reader.ReadString(value.ParamLength);
            return value;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8103_0x0023 value, IJT808Config config)
        {
            writer.WriteUInt32(value.ParamId);
            writer.Skip(1, out int skipPosition);
            writer.WriteString(value.ParamValue);
            int length = writer.GetCurrentPosition() - skipPosition - 1;
            writer.WriteByteReturn((byte)length, skipPosition);
        }
    }
}
