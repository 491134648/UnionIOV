using System.Threading;

namespace Union.Gateway.Metadata
{
    /// <summary>
    /// 原子计数器
    /// <see cref="Grpc.Core.Internal"/>
    /// </summary>
    internal class UnionAtomicCounter
    {
        long counter = 0;

        public UnionAtomicCounter(long initialCount = 0)
        {
            this.counter = initialCount;
        }

        public void Reset()
        {
            Interlocked.Exchange(ref counter, 0);
        }
        public long Increment()
        {
            return Interlocked.Increment(ref counter);
        }

        public long Add(long len)
        {
            return Interlocked.Add(ref counter,len);
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref counter);
        }
        public long Count
        {
            get
            {
                return Interlocked.Read(ref counter);
            }
        }
    }
}
