using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// FixedSizeReceiveFilter
    /// �̶������С��Э��������Э��֮��, ��������Ĵ�С������ͬ�ġ�
    /// ������ÿ����������9���ַ���ɵ��ַ�������"KILL BILL", ��Ӧ�������¾��������´�������ʵ��һ�����չ�����(ReceiveFilter):
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