using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// �ְ��ӿ�
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public interface IPackagePartReader<TPackageInfo>
    {
        bool Process(TPackageInfo package, ref SequenceReader<byte> reader, out IPackagePartReader<TPackageInfo> nextPartReader, out bool needMoreData);
    }
}