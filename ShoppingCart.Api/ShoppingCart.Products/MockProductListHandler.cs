using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShoppingCart.Products
{
    /// <summary>
 
    /// </summary>
    public class MockProductListHandler : IProductListHandler
    {
        
        private const int Seed = 12345;

        
        private const int ProductCount = 10;

        private readonly Faker<Product> _faker;
        ProductListQuery lstquery = new ProductListQuery();
        public MockProductListHandler()
        {
            var productId = 1;

            _faker = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(p => p.ID, _ => productId++)
                .RuleFor(p => p.Name, f => f.PickRandom(lstquery.product))
                .RuleFor(p => p.Description, f => f.PickRandom(lstquery.text))
                .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(4.99m, 99.99m), 2))
                .RuleFor(p => p.ImageUrl, f => f.PickRandom(lstquery.image));
        }

        public IEnumerable<Product> Handle(ProductListQuery query)
        {
            return _faker.UseSeed(Seed).Generate(ProductCount);
        }
    }
}
