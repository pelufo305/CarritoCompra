using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShoppingCart.Products
{
    /// <summary>
    /// A mock handler for product data, using the Bogus library: https://github.com/bchavez/Bogus.
    /// </summary>
    public class MockProductListHandler : IProductListHandler
    {
        // for demo purposes, we want the same mocked data upon each invocation.
        private const int Seed = 12345;

        // for the demo, we will set a number of products to generate.
        private const int ProductCount = 10;

        private readonly Faker<Product> _faker;

        public MockProductListHandler()
        {
            var productId = 1;

            _faker = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(p => p.ID, _ => productId++)
                .RuleFor(p => p.Name, "Sa22ndwich Especial "+ productId++)
                .RuleFor(p => p.Description, "Sandwich realizado con pan integral carnes final y verduras")
                .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(4.99m, 99.99m), 2))
                .RuleFor(p => p.ImageUrl,);
        }

        public IEnumerable<Product> Handle(ProductListQuery query)
        {
            return _faker.UseSeed(Seed).Generate(ProductCount);
        }
    }
}
