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
                    name: faker.Commerce.ProductName(),
                    description: faker.Random.Words(),
                    active: faker.Random.Bool(),
                    value: faker.Finance.Amount(1, decimal.MaxValue),
                    categoryId: Guid.NewGuid(),
                    image: faker.Image.PicsumUrl(),
                    dimensions: new Dimensions(height: 1, width: 1, depth: 1)));
        }

        public void Dispose()
        {
        }
    }
}