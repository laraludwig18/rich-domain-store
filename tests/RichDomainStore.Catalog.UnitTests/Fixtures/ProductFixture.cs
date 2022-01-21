using System;
using Bogus;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.ValueObjects;
using Xunit;

namespace RichDomainStore.Catalog.UnitTests.Fixtures
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
                    value: faker.Finance.Amount(min: 1),
                    categoryId: Guid.NewGuid(),
                    image: faker.Image.PicsumUrl(),
                    dimensions: new Dimensions(height: 1, width: 1, depth: 1)));
        }

        public void Dispose()
        {
        }
    }
}