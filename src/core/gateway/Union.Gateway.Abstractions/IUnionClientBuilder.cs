using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Gateway.Abstractions
{
    public interface IUnionClientBuilder
    {
        IJT808Builder JT808Builder { get; }
        IJT808Builder Builder();
    }
}
