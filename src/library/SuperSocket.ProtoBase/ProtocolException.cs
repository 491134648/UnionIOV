using System;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// –≠“È“Ï≥£
    /// </summary>
    public class ProtocolException : Exception
    {
        public ProtocolException(string message, Exception exception)
            : base(message, exception)
        {

        }

        public ProtocolException(string message)
            : base(message)
        {

        }
    }
}