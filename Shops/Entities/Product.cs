namespace Shops.Entities
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool IsEqual(Product other)
        {
            return Name == other.Name;
        }
    }
}