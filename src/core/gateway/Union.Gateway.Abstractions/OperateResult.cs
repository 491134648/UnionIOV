using System.Collections.Generic;

namespace Union.Gateway.Abstractions
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class OperateResult
    {
        public OperateResult(int code)
        {
            Code = code;
        }
        public OperateResult(int code,string message)
        {
            Code = code;
            Message = message;
        }
        public OperateResult(int code,string message,KeyValuePair<string,object> keyValue)
        {
            Code = code;
            Message = message;
            Extras.Add(keyValue.Key,keyValue.Value);
        }
        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public Dictionary<string, object> Extras { get; set; } = new Dictionary<string, object>();
    }
}
