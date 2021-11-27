using System;
using System.Runtime.Serialization;

namespace BackupsExtra.Loggers
{
    [DataContract]
    [KnownType(typeof(FileLogger))]
    [KnownType(typeof(ConsoleLogger))]
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