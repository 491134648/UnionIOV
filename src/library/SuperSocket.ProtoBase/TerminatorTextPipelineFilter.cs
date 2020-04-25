using System;
using System.Buffers;
using System.Text;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// 
    /// </summary>
    public class TerminatorTextPipelineFilter : TerminatorPipelineFilter<TextPackageInfo>
    {

        public TerminatorTextPipelineFilter(ReadOnlyMemory<byte> terminator)
            : base(terminator)
        {

        }

        protected override TextPackageInfo DecodePackage(ReadOnlySequence<byte> buffer)
        {
            return new TextPackageInfo { Text = buffer.GetString(Encoding.UTF8) };
        }
    }
}
