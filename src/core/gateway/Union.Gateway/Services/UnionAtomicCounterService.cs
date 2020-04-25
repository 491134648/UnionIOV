using Union.Gateway.Metadata;

namespace Union.Gateway.Services
{
    /// <summary>
    /// 计数包服务
    /// </summary>
    public class UnionAtomicCounterService
    {
        private readonly UnionAtomicCounter MsgSuccessCounter;

        private readonly UnionAtomicCounter MsgFailCounter;

        public UnionAtomicCounterService()
        {
            MsgSuccessCounter=new UnionAtomicCounter();
            MsgFailCounter = new UnionAtomicCounter();
        }

        public void Reset()
        {
            MsgSuccessCounter.Reset();
            MsgFailCounter.Reset();
        }

        public long MsgSuccessIncrement()
        {
            return MsgSuccessCounter.Increment();
        }

        public long MsgSuccessCount
        {
            get
            {
                return MsgSuccessCounter.Count;
            }
        }

        public long MsgFailIncrement()
        {
            return MsgFailCounter.Increment();
        }

        public long MsgFailCount
        {
            get
            {
                return MsgFailCounter.Count;
            }
        }
    }
}
