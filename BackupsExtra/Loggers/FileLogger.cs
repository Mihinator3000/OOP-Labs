using System.IO;
using System.Runtime.Serialization;

namespace BackupsExtra.Loggers
{
    [DataContract]
    public class FileLogger : AbstractLogger
    {
        [DataMember(Name = "Path")]
        private readonly string _path;
        public FileLogger()
            : this("log.txt")
        {
        }

        public FileLogger(string path)
        {
            _path = path;
        }

        public override void Log(string message)
        {
            using StreamWriter writer = File.AppendText(_path);
            writer.WriteLine(GetMessage(message));
            writer.Close();
        }
    }
}