namespace Isu.Entities
{
    public interface IEqual<in T>
    {
        public bool IsEqual(T other);
    }
}
