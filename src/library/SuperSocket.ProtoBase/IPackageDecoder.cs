using System.Buffers;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public interface IPackageDecoder<out TPackageInfo>
        where TPackageInfo : class
    {
        TPackageInfo Decode(ReadOnlySequence<byte> buffer, object context);
    }
}