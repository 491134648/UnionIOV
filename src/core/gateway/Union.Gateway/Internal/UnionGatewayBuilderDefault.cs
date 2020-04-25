using JT808.Protocol;
using Union.Gateway.Abstractions;

namespace Union.Gateway.Internal
{
    public class UnionGatewayBuilderDefault : IUnionGatewayBuilder
    {
        public IJT808Builder JT808Builder { get; }

        public UnionGatewayBuilderDefault(IJT808Builder builder)
        {
            JT808Builder = builder;
        }

        public IJT808Builder Builder()
        {
            return JT808Builder;
        }
    }
}