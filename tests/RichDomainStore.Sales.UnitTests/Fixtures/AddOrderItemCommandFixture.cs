using System;
using Bogus;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Entities;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Fixtures
{
    [CollectionDefinition(nameof(AddOrderItemCommandCollection))]
    public class AddOrderItemCommandCollection : ICollectionFixture<AddOrderItemCommandFixture>
    { }

    public class AddOrderItemCommandFixture : IDisposable
    {
        public AddOrderItemCommand GenerateValidCommand(Guid productId = default(Guid))
        {
            return new Faker<AddOrderItemCommand>("pt_BR")
                .CustomInstantiator(faker => new AddOrderItemCommand(
                    customerId: Guid.NewGuid(),
                    productId: productId == Guid.Empty ? Guid.NewGuid() : productId,
                    productName: faker.Commerce.ProductName(),
                    quantity: faker.Random.Int(min: OrderItem.MinItemQuantity, max: OrderItem.MaxItemQuantity),
                    value: faker.Finance.Amount(min: 1)));
        }

        public AddOrderItemCommand GenerateInvalidCommand(int itemQuantity = 1)
        {
            return new Faker<AddOrderItemCommand>("pt_BR")
                .CustomInstantiator(faker => new AddOrderItemCommand(
                    customerId: Guid.Empty,
                    productId: Guid.Empty,
                    productName: string.Empty,
                    quantity: itemQuantity,
                    value: 0));
        }

        public void Dispose()
        {
        }
    }
}