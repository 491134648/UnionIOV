using System.Collections.Concurrent;
using Union.Gateway.Abstractions.Enums;

namespace Union.Gateway.Services
{
    public class UnionAtomicCounterServiceFactory
    {
        private readonly ConcurrentDictionary<TransportProtocolType, UnionAtomicCounterService> cache;

        public UnionAtomicCounterServiceFactory()
        {
            cache = new ConcurrentDictionary<TransportProtocolType, UnionAtomicCounterService>();
        }

        public UnionAtomicCounterService Create(TransportProtocolType type)
        {
            if(cache.TryGetValue(type,out var service))
            {
                return service;
            }
            else
            {
                var serviceNew = new UnionAtomicCounterService();
                cache.TryAdd(type, serviceNew);
                return serviceNew;
            }
        }
    }
}
