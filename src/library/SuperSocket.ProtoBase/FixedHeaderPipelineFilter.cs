using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// FixedHeaderReceiveFilter - 头部格式固定并且包含内容长度的协议
    /// 这种协议将一个请求定义为两大部分, 第一部分定义了包含第二部分长度等等基础信息. 我们通常称第一部分为头部.
    /// 例如, 我们有一个这样的协议: 头部包含 6 个字节, 前 4 个字节用于存储请求的名字, 后两个字节用于代表请求体的长度:
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public abstract class FixedHeaderPipelineFilter<TPackageInfo> : FixedSizePipelineFilter<TPackageInfo>
        where TPackageInfo : class
    {
        private bool _foundHeader;
        private readonly int _headerSize;
        private int _totalSize;

        protected FixedHeaderPipelineFilter(int headerSize)
            : base(headerSize)
        {
            _headerSize = headerSize;
        }
        public override TPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (!_foundHeader)
            {
                if (reader.Length < _headerSize)
                    return null;                
                
                var header = reader.Sequence.Slice(0, _headerSize);
                var bodyLength = GetBodyLengthFromHeader(header);
                
                if (bodyLength < 0)
                    throw new ProtocolException("Failed to get body length from the package header.");
                
                if (bodyLength == 0)
                    return DecodePackage(header);
                
                _foundHeader = true;
                _totalSize = _headerSize + bodyLength;
            }

            var totalSize = _totalSize;

            if (reader.Length < totalSize)
                return null;

            var pack = reader.Sequence.Slice(0, totalSize);

            try
            {
                return DecodePackage(pack);
            }
            finally
            {
                reader.Advance(totalSize);
            } 
        }
        
        protected abstract int GetBodyLengthFromHeader(ReadOnlySequence<byte> buffer);

        public override void Reset()
        {
            _foundHeader = false;
            _totalSize = 0;
        }
    }
}