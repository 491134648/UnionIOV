﻿using JT808.Protocol.Enums;

namespace JT808.Protocol.Extensions
{
    public static partial class JT808PackageExtensions
    {
        public static JT808Package Create<TJT808Bodies>(this JT808MsgId msgId, string terminalPhoneNo, TJT808Bodies bodies)
            where TJT808Bodies : JT808Bodies
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = (ushort)msgId,
                    TerminalPhoneNo = terminalPhoneNo,
                },
                Bodies = bodies
            };
            return jT808Package;
        }

        public static JT808Package Create(this JT808MsgId msgId, string terminalPhoneNo)
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = (ushort)msgId,
                    TerminalPhoneNo = terminalPhoneNo,
                }
            };
            return jT808Package;
        }

        public static JT808Package CreateCustomMsgId<TJT808Bodies>(this ushort msgId, string terminalPhoneNo, TJT808Bodies bodies)
            where TJT808Bodies : JT808Bodies
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = msgId,
                    TerminalPhoneNo = terminalPhoneNo
                },
                Bodies = bodies
            };
            return jT808Package;
        }

        public static JT808Package CreateCustomMsgId(this ushort msgId, string terminalPhoneNo)
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = msgId,
                    TerminalPhoneNo = terminalPhoneNo
                }
            };
            return jT808Package;
        }

        public static JT808Package Create2019<TJT808Bodies>(this JT808MsgId msgId, string terminalPhoneNo, TJT808Bodies bodies)
            where TJT808Bodies : JT808Bodies
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = (ushort)msgId,
                    TerminalPhoneNo = terminalPhoneNo,
                },
                Bodies = bodies
            };
            jT808Package.Header.MessageBodyProperty.VersionFlag = true;
            return jT808Package;
        }

        public static JT808Package Create2019(this JT808MsgId msgId, string terminalPhoneNo)
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = (ushort)msgId,
                    TerminalPhoneNo = terminalPhoneNo,
                }
            };
            jT808Package.Header.MessageBodyProperty.VersionFlag = true;
            return jT808Package;
        }

        public static JT808Package CreateCustomMsgId2019<TJT808Bodies>(this ushort msgId, string terminalPhoneNo, TJT808Bodies bodies)
            where TJT808Bodies : JT808Bodies
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = msgId,
                    TerminalPhoneNo = terminalPhoneNo
                },
                Bodies = bodies
            };
            jT808Package.Header.MessageBodyProperty.VersionFlag = true;
            return jT808Package;
        }

        public static JT808Package CreateCustomMsgId2019(this ushort msgId, string terminalPhoneNo)
        {
            JT808Package jT808Package = new JT808Package
            {
                Header = new JT808Header
                {
                    MsgId = msgId,
                    TerminalPhoneNo = terminalPhoneNo
                }
            };
            jT808Package.Header.MessageBodyProperty.VersionFlag = true;
            return jT808Package;
        }
    }
}
