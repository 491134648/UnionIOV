using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// FixedSizeReceiveFilter
    /// 固定请求大小的协议在这种协议之中, 所有请求的大小都是相同的。
    /// 如果你的每个请求都是有9个字符组成的字符串，如"KILL BILL", 你应该做的事就是想如下代码这样实现一个接收过滤器(ReceiveFilter):
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public class FixedSizePipelineFilter<TPackageInfo> : PipelineFilterBase<TPackageInfo>
        where TPackageInfo : class
    {
        private int _size;

        protected FixedSizePipelineFilter(int size)
        {
            _size = size;
        }
        
        public override TPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (reader.Length < _size)
                return null;

            var pack = reader.Sequence.Slice(0, _size);

            try
            {
                return DecodePackage(pack);
            }
            finally
            {
                reader.Advance(_size);
            }
        }
    }
}