﻿using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Linq;
using Xunit;

namespace JT808.Protocol.Test.MessageBody
{
    public class JT808_0x0A00Test
    {
        JT808Serializer JT808Serializer = new JT808Serializer();
        byte[] N;
        public JT808_0x0A00Test()
        {

            N = Enumerable.Range(0, 128).Select(s => Convert.ToByte(s)).ToArray();
        }

        [Fact]
        public void Test1()
        {
            JT808_0x0A00 jT808_0X0A00 = new JT808_0x0A00
            {
                E = 128,
                N = N
            };
            string hex = JT808Serializer.Serialize(jT808_0X0A00).ToHexString();
            Assert.Equal("00000080000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F404142434445464748494A4B4C4D4E4F505152535455565758595A5B5C5D5E5F606162636465666768696A6B6C6D6E6F707172737475767778797A7B7C7D7E7F", hex);
        }

        [Fact]
        public void Test2()
        {
            byte[] bytes = "00000080000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F404142434445464748494A4B4C4D4E4F505152535455565758595A5B5C5D5E5F606162636465666768696A6B6C6D6E6F707172737475767778797A7B7C7D7E7F".ToHexBytes();
            JT808_0x0A00 jT808_0X0A00 = JT808Serializer.Deserialize<JT808_0x0A00>(bytes);
            Assert.Equal((uint)128, jT808_0X0A00.E);
            Assert.Equal(N, jT808_0X0A00.N);
        }
    }
}
