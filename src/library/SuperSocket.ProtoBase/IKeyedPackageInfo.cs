using System;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IKeyedPackageInfo<TKey>
    {
        TKey Key { get; }
    }
}
