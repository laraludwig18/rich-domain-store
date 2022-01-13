using System;
using Bogus;
using RichDomainStore.Catalog.Domain.Entities;
using Xunit;

namespace RichDomainStore.Catalog.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductFixture>
    { }

    public class ProductFixture : IDisposable
    {
        public Product GenerateValidProduct()
        {
            return new Faker<Product>("pt_BR")
                .CustomInstantiator(faker => new Product(
                    faker.Commerce.ProductName(),
                    faker.Random.Words(),
                    faker.Random.Bool(),
                    faker.Finance.Amount(1, decimal.MaxValue),
                    Guid.NewGuid(),
                    faker.Image.PicsumUrl(),
                    new Dimensions(1, 1, 1)));
        }

        public void Dispose()
        {
        }
    }
}