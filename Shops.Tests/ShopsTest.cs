using System;
using NUnit.Framework;
using Shops.Entities;
using Shops.Services;
using Shops.Tools;

namespace Shops.Tests
{
    [TestFixture]
    public class Tests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void PersonBoughtProducts_MoneyAndProductCounterChanged()
        {
            const int moneyBefore = 100;
            const int productPrice = 30;
            const int productCount = 5;
            const int productToBuyCount = 3;

            var person = new Person("SomeGuy", moneyBefore);

            Shop shop = _shopManager.Create("Shop name", "22, Olive Street");

            shop.AddProducts(new ProductInfo(
                "Carrot",
                    productCount,
                    productPrice));

            Assert.IsTrue(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));

            Assert.AreEqual(moneyBefore - productPrice * productToBuyCount, person.Money);

            Assert.AreEqual(productCount - productToBuyCount,
                _shopManager.GetProductInfo(shop.ShopGuid,
                    new Product("Carrot")).Count);
        }

        [Test]
        public void NotEnoughMoneyToBuyProduct_ProductNotSold()
        {
            const int moneyBefore = 10;
            const int productPrice = 30;
            const int productCount = 10;
            const int productToBuyCount = 3;

            var person = new Person("SomeGuy", moneyBefore);

            Shop shop = _shopManager.Create("Shop name", "22, Olive Street");

            _shopManager.AddProducts(shop.ShopGuid,
               new ProductInfo("Carrot",
                   productCount,
                   productPrice));

            Assert.False(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));
        }

        [Test]
        public void NotEnoughProductToBuy_ProductNotSold()
        {
            const int moneyBefore = 1000;
            const int productPrice = 30;
            const int productCount = 10;
            const int productToBuyCount = 100;

            var person = new Person("SomeGuy", moneyBefore);

            Shop shop = _shopManager.Create("Shop name", "22, Olive Street");

            _shopManager.AddProducts(shop.ShopGuid,
               new ProductInfo("Carrot",
                   productCount,
                   productPrice));

            Assert.False(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));
        }

        [Test]
        public void ProductBoughtFromCheapestStore()
        {
            const int moneyBefore = 1000;
            const int productCount = 10;
            const int productToBuyCount = 7;

            const int productPriceFirstStore = 40;
            const int productPriceSecondShore = 30;

            var person = new Person("SomeGuy", moneyBefore);

            Shop firstShop = _shopManager.Create("Shop name", "22, Olive Street");
            _shopManager.AddProducts(firstShop.ShopGuid,
               new ProductInfo("Carrot",
                   productCount,
                   productPriceFirstStore));

            Shop secondShop = _shopManager.Create("Shop name", "22, Olive Street");
            _shopManager.AddProducts(secondShop.ShopGuid,
               new ProductInfo("Carrot",
                   productCount,
                   productPriceSecondShore));

            Assert.True(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));

            Assert.AreEqual(productCount - productToBuyCount,
                _shopManager.GetProductInfo(secondShop.ShopGuid,
                    new Product("Carrot")).Count);
        }

        [Test]
        public void ProductPriceChanged_PersonCouldBuyProduct()
        {
            const int moneyBefore = 100;
            const int productCount = 5;
            const int productToBuyCount = 3;

            const int productPrice = 40;
            const int newProductPrice = 30;

            var person = new Person("SomeGuy", moneyBefore);

            Shop shop = _shopManager.Create("Shop name", "22, Olive Street");

            _shopManager.AddProducts(shop.ShopGuid,
                new ProductInfo("Carrot",
                    productCount,
                    productPrice));

            Assert.IsFalse(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));

            shop.ChangePrice( 
                new Product("Carrot"), 
                newProductPrice);

            Assert.IsTrue(_shopManager.TryBuy(person,
                new ProductInfo("Carrot",
                    productToBuyCount)));
        }

        [Test]
        public void NoShopByGuid_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                 _shopManager.GetShop(new Guid());
            });
        }

        [Test]
        public void NoProductFound_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                Shop shop = _shopManager.Create("Shop name", "22, Olive Street");

                _shopManager.AddProducts(shop.ShopGuid,
                new ProductInfo("Carrot",
                    10,
                    10));

                _shopManager.GetProductInfo(shop.ShopGuid,
                    new Product("Potato"));
            });
        }
    }
}
