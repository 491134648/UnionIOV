using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Union.Gateway.Traffic
{
    public class UnionTrafficDefault : IUnionTraffic
    {
        private ConcurrentDictionary<string, long> dict = new ConcurrentDictionary<string, long>();

        public long Get(string key)
        {
            long value;
            dict.TryGetValue(key, out value);
            return value;
        }

        public List<(string, long)> GetAll()
        {
            return dict.Select(s => (s.Key, s.Value)).ToList();
        }

        public long Increment(string terminalNo, string field, int len)
        {
            return dict.AddOrUpdate($"{terminalNo}_{field}", len, (id, count) => count + len);
        }
    }
}
