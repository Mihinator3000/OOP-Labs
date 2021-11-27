using System;
using System.Runtime.Serialization;

namespace BackupsExtra.Loggers
{
    [DataContract]
    public class ConsoleLogger : AbstractLogger
    {
        public override void Log(string message)
        {
            Console.WriteLine(GetMessage(message));
        }
    }
}