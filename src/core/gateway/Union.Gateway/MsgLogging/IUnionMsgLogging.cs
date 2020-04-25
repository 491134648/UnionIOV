namespace Union.Gateway.MsgLogging
{
    /// <summary>
    /// 808数据上下行日志接口
    /// </summary>
    public interface IUnionMsgLogging
    {
        void Processor((string TerminalNo, byte[] Data) parameter, UnionMsgLoggingType jT808MsgLoggingType);
    }
}
