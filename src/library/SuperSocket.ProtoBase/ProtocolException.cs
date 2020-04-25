using System;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// Э���쳣
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