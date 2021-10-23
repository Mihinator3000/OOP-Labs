namespace Backups.Entities.Files
{
    public interface IJobObject
    {
        string Path { get; }

        string Name { get; }

        string NameWithoutExtension { get; }

        bool Exists();
    }
}