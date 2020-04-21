﻿using JT808.Protocol.Formatters;
using JT808.Protocol.MessagePack;
using System;

namespace JT808.Protocol.MessageBody
{
    /// <summary>
    /// 存储多媒体数据上传命令
    /// 0x8803
    /// </summary>
    public class JT808_0x8803 : JT808Bodies, IJT808MessagePackFormatter<JT808_0x8803>
    {
        public override ushort MsgId { get; } = 0x8803;
        /// <summary>
        /// 多媒体类型 
        /// <see cref="JT808.Protocol.Enums.JT808MultimediaType"/>
        /// 0：图像；1：音频；2：视频
        /// </summary>
        public byte MultimediaType { get; set; }
        /// <summary>
        /// 通道 ID
        /// </summary>
        public byte ChannelId { get; set; }
        /// <summary>
        /// 事件项编码
        /// <see cref="JT808.Protocol.Enums.JT808EventItemCoding"/>
        /// 0：平台下发指令；1：定时动作；2：抢劫报警触发；3：碰撞侧翻报警触发；其他保留 
        /// </summary>
        public byte EventItemCoding { get; set; }
        /// <summary>
        /// 起始时间
        /// YY-MM-DD-hh-mm-ss
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// YY-MM-DD-hh-mm-ss
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 删除标志
        /// <see cref="JT808.Protocol.Enums.JT808MultimediaDeleted"/>
        /// </summary>
        public byte MultimediaDeleted { get; set; }
        public JT808_0x8803 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8803 jT808_0X8803 = new JT808_0x8803();
            jT808_0X8803.MultimediaType = reader.ReadByte();
            jT808_0X8803.ChannelId = reader.ReadByte();
            jT808_0X8803.EventItemCoding = reader.ReadByte();
            jT808_0X8803.StartTime = reader.ReadDateTime6();
            jT808_0X8803.EndTime = reader.ReadDateTime6();
            jT808_0X8803.MultimediaDeleted = reader.ReadByte();
            return jT808_0X8803;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8803 value, IJT808Config config)
        {
            writer.WriteByte(value.MultimediaType);
            writer.WriteByte(value.ChannelId);
            writer.WriteByte(value.EventItemCoding);
            writer.WriteDateTime6(value.StartTime);
            writer.WriteDateTime6(value.EndTime);
            writer.WriteByte(value.MultimediaDeleted);
        }
    }
}
