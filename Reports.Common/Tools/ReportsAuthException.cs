using System;
using System.Runtime.Serialization;

namespace Reports.Common.Tools
{
    public class ReportsAuthException : Exception
    {
        public ReportsAuthException()
        {
        }

        public ReportsAuthException(string message)
            : base(message)
        {
        }

        public ReportsAuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReportsAuthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}