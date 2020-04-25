namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// CommandLine Pipeline Filter
    /// 命令行协议是一种被广泛应用的协议。一些成熟的协议如 Telnet, SMTP, POP3 和 FTP 都是基于命令行协议的。 
    /// 在SuperSocket 中， 如果你没有定义自己的协议，SuperSocket 将会使用命令行协议, 这会使这样的协议的开发变得很简单。
    /// 命令行协议定义了每个请求必须以回车换行结尾 "\r\n"。
    /// </summary>
    public class CommandLinePipelineFilter : TerminatorPipelineFilter<StringPackageInfo>
    {
        public CommandLinePipelineFilter()
            : base(new[] { (byte)'\r', (byte)'\n' })
        {

        }
    }
}
