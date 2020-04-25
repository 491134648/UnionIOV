using JT808.Protocol;
using Union.Gateway.Abstractions;

namespace Union.Gateway.Internal
{
    public class UnionNormalGatewayBuilderDefault : IUnionNormalGatewayBuilder
    {
        public IJT808Builder JT808Builder { get; }

        public UnionNormalGatewayBuilderDefault(IJT808Builder builder)
        {
            JT808Builder = builder;
        }

        public IJT808Builder Builder()
        {
            return JT808Builder;
        }
    }
}