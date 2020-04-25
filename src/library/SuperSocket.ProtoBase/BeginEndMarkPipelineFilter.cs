using System;
using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// BeginEndMark Pipeline Filter
    /// 在这类协议的每个请求之中 都有固定的开始和结束标记。例如, 我有个协议，它的所有消息都遵循这种格式 "!xxxxxxxxxxxxxx$"。
    /// 因此，在这种情况下， "!" 是开始标记， "$" 是结束标记，于是你的接受过滤器可以定义成这样
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public abstract class BeginEndMarkPipelineFilter<TPackageInfo> : PipelineFilterBase<TPackageInfo>
        where TPackageInfo : class
    {
        private readonly ReadOnlyMemory<byte> _beginMark;

        private readonly ReadOnlyMemory<byte> _endMark;

        private bool _foundBeginMark;
        
        protected BeginEndMarkPipelineFilter(ReadOnlyMemory<byte> beginMark, ReadOnlyMemory<byte> endMark)
        {
            _beginMark = beginMark;
            _endMark = endMark;
        }
        
        public override TPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (!_foundBeginMark)
            {
                var beginMark = _beginMark.Span;

                if (!reader.IsNext(beginMark, advancePast: true))
                {
                    throw new ProtocolException("Invalid beginning part of the package.");
                }
                _foundBeginMark = true;
            }

            var endMark =  _endMark.Span;

            if (!reader.TryReadTo(out ReadOnlySequence<byte> pack, endMark, advancePastDelimiter: false))
            {
                return null;
            }

            reader.Advance(endMark.Length);
            return DecodePackage(pack);
        }

        public override void Reset()
        {
            _foundBeginMark = false;
        }
    }
}