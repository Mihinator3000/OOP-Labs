using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private readonly List<ProductInfo> _productsInfo = new ();

        private string _address;

        public Shop(string name, string address)
        {
            Name = name;
            _address = address;
        }

        public string Name { get; }

        public Guid ShopGuid { get; } = Guid.NewGuid();

        public void AddProducts(params ProductInfo[] productsInfo)
        {
            foreach (ProductInfo productInfo in productsInfo)
            {
                ProductInfo shopProductInfo = _productsInfo.FirstOrDefault(u =>
                u.Product.IsEqual(productInfo.Product));
                if (shopProductInfo == null)
                {
                    _productsInfo.Add(productInfo);
                    continue;
                }

                int index = _productsInfo.IndexOf(shopProductInfo);
                _productsInfo[index].Count += productInfo.Count;
            }
        }

        public ProductInfo GetProductInfo(Product product)
        {
            ProductInfo productInfo = _productsInfo.FirstOrDefault(u =>
                u.Product.IsEqual(product));

            if (productInfo is null)
                throw new ShopException(product.Name);

            return productInfo;
        }

        public int? TryGetProductPrice(params ProductInfo[] productsInfo)
        {
            int price = 0;

            foreach (ProductInfo productInfo in productsInfo)
            {
                ProductInfo shopProduct = _productsInfo.FirstOrDefault(u =>
                    u.Product.IsEqual(productInfo.Product) &&
                    u.Count >= productInfo.Count);

                if (shopProduct == null)
                    return null;

                price += shopProduct.Price * productInfo.Count;
            }

            return price;
        }

        public void RemoveProducts(ProductInfo productInfo)
        {
            int index = _productsInfo.IndexOf(
                GetProductInfo(productInfo.Product));

            _productsInfo[index].Count -= productInfo.Count;
        }

        public void ChangePrice(Product product, int newPrice)
        {
            int index = _productsInfo.IndexOf(
                GetProductInfo(product));

            _productsInfo[index].Price = newPrice;
        }
    }
}