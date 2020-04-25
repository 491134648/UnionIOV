using Microsoft.Extensions.Logging;

namespace Union.Gateway.SessionNotice
{
    public class UnionSessionNoticeService
    {
        protected ILogger logger { get; }
        public UnionSessionNoticeService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("JT808SessionNoticeService");
        }
        public virtual void Processor((string Notice, string TerminalNo) parameter)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"{parameter.Notice}-{parameter.TerminalNo}");
            }
        }
    }
}
