using System;
using Bogus;
using RichDomainStore.Sales.Domain.Entities;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(OrderCollection))]
    public class OrderCollection : ICollectionFixture<OrderFixture>
    { }

    public class OrderFixture : IDisposable
    {
        public Order GenerateDraftOrder()
        {
            return Order.OrderFactory.NewDraft(Guid.NewGuid());
        }

        public OrderItem GenerateValidOrderItem(int itemQuantity, decimal value = 1)
        {
            return new Faker<OrderItem>("pt_BR")
                .CustomInstantiator(faker => new OrderItem(
                    Guid.NewGuid(),
                    faker.Commerce.ProductName(),
                    itemQuantity,
                    value));
        }

        public void Dispose()
        {
        }
    }
}