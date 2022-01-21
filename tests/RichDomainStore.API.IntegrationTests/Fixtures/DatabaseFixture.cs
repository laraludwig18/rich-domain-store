using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.ValueObjects;
using Xunit;

namespace RichDomainStore.API.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(DatabaseCollection))]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

    public class DatabaseFixture : IDisposable
    {
        private List<Category> _categories;
        public IReadOnlyCollection<Category> Categories => _categories?.AsReadOnly();
        private List<Product> _products;
        public IReadOnlyCollection<Product> Products => _products?.AsReadOnly();
        
        public DatabaseFixture()
        {
            _categories = GenerateValidCategory(1);
            _products = GenerateValidProduct(1);
        }

        public List<Category> GenerateValidCategory(int quantity = 1)
        {
            return new Faker<Category>("pt_BR")
                .CustomInstantiator(faker => new Category(
                    name: faker.Random.Word(),
                    code: faker.Random.Int(min: 1)))
                .Generate(quantity);
        }

        public List<Product> GenerateValidProduct(int quantity = 1)
        {
            var products = new Faker<Product>("pt_BR")
                .CustomInstantiator(faker => new Product(
                    name: faker.Commerce.ProductName(),
                    description: faker.Random.Words(),
                    active: faker.Random.Bool(),
                    value: faker.Finance.Amount(min: 1),
                    categoryId: _categories.FirstOrDefault().Id,
                    image: faker.Image.PicsumUrl(),
                    dimensions: new Dimensions(height: 1, width: 1, depth: 1)))
                .Generate(quantity);

            products.ForEach(p => p.ReStock(1000));

            return products;
        }

        public void Dispose()
        {
            _products = null;
            _categories = null;
        }
    }
}