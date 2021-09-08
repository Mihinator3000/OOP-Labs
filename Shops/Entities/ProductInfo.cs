namespace Shops.Entities
{
    public class ProductInfo
    {
        public ProductInfo(string name)
        {
            Product = new Product(name);
        }

        public ProductInfo(string name, int count)
            : this(name)
        {
            Count = count;
        }

        public ProductInfo(string name, int count, int price)
            : this(name, count)
        {
            Price = price;
        }

        public Product Product { get; }

        public int Price { get; set; }

        public int Count { get; set; }
    }
}