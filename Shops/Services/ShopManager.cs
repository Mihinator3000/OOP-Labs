using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager
    {
        private readonly List<Shop> _shops = new ();

        private readonly List<Product> _products = new ();

        public Shop Create(string name, string address)
        {
            var shop = new Shop(name, address);
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name)
        {
            var product = new Product(name);
            _products.Add(product);
            return product;
        }

        public Product FindProduct(string name)
        {
            return _products.FirstOrDefault(u => u.Name == name);
        }

        public Shop GetShop(Guid shopGuid)
        {
            Shop shop = _shops.FirstOrDefault(u => u.ShopGuid == shopGuid);
            if (shop is null)
                throw new ShopException("Invalid guid");

            return shop;
        }

        public List<Shop> FindShops(string name)
        {
            return _shops.Where(u => u.Name == name).ToList();
        }

        public ProductInfo GetProductInfo(Guid shopGuid, Product product)
        {
            return GetShop(shopGuid).GetProductInfo(product);
        }

        public void ChangePrice(Guid shopGuid, Product product, int newPrice)
        {
            int index = _shops.IndexOf(GetShop(shopGuid));

            _shops[index].ChangePrice(product, newPrice);
        }

        public void AddProducts(Guid shopGuid, params ProductInfo[] productsInfo)
        {
            int index = _shops.IndexOf(GetShop(shopGuid));

            _shops[index].AddProducts(productsInfo);
        }

        public bool TryBuy(Person buyer, params ProductInfo[] productsInfo)
        {
            int minPrice = int.MaxValue;
            Shop minPriceShop = null;
            foreach (Shop shop in _shops)
            {
                int? price = shop.TryGetProductPrice(productsInfo);
                if (price is null ||
                    price >= minPrice)
                    continue;

                minPrice = (int)price;
                minPriceShop = shop;
            }

            if (minPriceShop is null ||
                minPrice > buyer.Money)
                return false;

            Buy(minPriceShop, minPrice, buyer, productsInfo);
            return true;
        }

        private void Buy(Shop shop, int minPrice, Person buyer, params ProductInfo[] productsInfo)
        {
            int shopIndex = _shops.IndexOf(shop);
            foreach (ProductInfo productInfo in productsInfo)
                _shops[shopIndex].RemoveProducts(productInfo);

            buyer.Money -= minPrice;
        }
    }
}