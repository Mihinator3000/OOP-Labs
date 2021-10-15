using Backups.Tools;

namespace Backups.Entities.Files
{
    public class JobObject
    {
        public JobObject(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string Name =>
            System.IO.Path.GetFileName(Path);

        public string NameWithoutExtension =>
            System.IO.Path.GetFileNameWithoutExtension(Name)
            ?? throw new BackupsException(Name);
    }
}