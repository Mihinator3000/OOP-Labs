using System;
using System.Runtime.Serialization;

namespace BackupsExtra.Loggers
{
    [DataContract, KnownType(typeof(ConsoleLogger)), KnownType(typeof(FileLogger))]
    public abstract class AbstractLogger
    {
        [DataMember(Name = "RecordTime")]
        private bool _recordTime;

        public AbstractLogger SetTimeRecording()
        {
            _recordTime = true;
            return this;
        }

        public abstract void Log(string message);

        protected string GetMessage(string message)
        {
            return _recordTime
                ? $"{DateTime.Now} : {message}"
                : message;
        }
    }
}