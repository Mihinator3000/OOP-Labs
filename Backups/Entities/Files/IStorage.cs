namespace Backups.Entities.Files
{
    public interface IStorage
    {
        string FullPath();

        string FullPath(string filename);
    }
}