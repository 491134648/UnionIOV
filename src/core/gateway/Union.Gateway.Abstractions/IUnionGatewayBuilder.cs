using JT808.Protocol;

namespace Union.Gateway.Abstractions
{
    public interface IUnionGatewayBuilder
    {
        IJT808Builder JT808Builder { get; }
        IJT808Builder Builder();
    }
}
