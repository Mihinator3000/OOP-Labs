using System;
using System.Runtime.Serialization;

namespace Reports.Common.Tools
{
    public class ReportsDbException : Exception
    {
        public ReportsDbException()
        {
        }

        public ReportsDbException(string message)
            : base(message)
        {
        }

        public ReportsDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReportsDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}