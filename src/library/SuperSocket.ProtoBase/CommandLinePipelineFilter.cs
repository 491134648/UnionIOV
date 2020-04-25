namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// CommandLine Pipeline Filter
    /// ������Э����һ�ֱ��㷺Ӧ�õ�Э�顣һЩ�����Э���� Telnet, SMTP, POP3 �� FTP ���ǻ���������Э��ġ� 
    /// ��SuperSocket �У� �����û�ж����Լ���Э�飬SuperSocket ����ʹ��������Э��, ���ʹ������Э��Ŀ�����úܼ򵥡�
    /// ������Э�鶨����ÿ����������Իس����н�β "\r\n"��
    /// </summary>
    public class CommandLinePipelineFilter : TerminatorPipelineFilter<StringPackageInfo>
    {
        public CommandLinePipelineFilter()
            : base(new[] { (byte)'\r', (byte)'\n' })
        {

        }
    }
}
