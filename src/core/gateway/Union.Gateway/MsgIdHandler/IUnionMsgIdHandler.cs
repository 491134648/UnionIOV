namespace Union.Gateway.MsgIdHandler
{
    /// <summary>
    /// 消息Id处理程序
    /// </summary>
    public interface IUnionMsgIdHandler
    {
        void Processor((string TerminalNo, byte[] Data) parameter);
    }
}
