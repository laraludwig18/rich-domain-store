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
            return Order.OrderFactory.NewDraft(customerId: Guid.NewGuid());
        }

        public OrderItem GenerateValidOrderItem(Guid productId = default(Guid), int itemQuantity = 1, decimal value = 1)
        {
            return new Faker<OrderItem>("pt_BR")
                .CustomInstantiator(faker => new OrderItem(
                    productId: productId == Guid.Empty ? Guid.NewGuid() : productId,
                    productName: faker.Commerce.ProductName(),
                    quantity: itemQuantity,
                    value: value));
        }

        public void Dispose()
        {
        }
    }
}