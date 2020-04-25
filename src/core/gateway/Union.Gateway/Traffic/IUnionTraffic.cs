using System.Collections.Generic;

namespace Union.Gateway.Traffic
{
    /// <summary>
    /// 流量统计服务
    /// </summary>
    public interface IUnionTraffic
    {
        long Get(string key);
        long Increment(string terminalNo, string field, int len);
        List<(string, long)> GetAll();
    }

    
}
