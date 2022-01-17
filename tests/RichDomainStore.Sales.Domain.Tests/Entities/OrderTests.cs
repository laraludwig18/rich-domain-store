using System.Linq;
using FluentAssertions;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Tests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Entities
{
    [Collection(nameof(OrderCollection))]
    public class OrderTests
    {
        private readonly OrderFixture _orderFixture;
        private readonly Order _draftOrder;
        public OrderTests(OrderFixture orderFixture)
        {
            _orderFixture = orderFixture;
            _draftOrder = _orderFixture.GenerateDraftOrder();
        }

        [Fact]
        public void AddItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: 2, value: 100);

            // Act
            _draftOrder.AddItem(orderItem);

            // Assert
            _draftOrder.TotalValue.Should().Be(200);
        }

        [Fact]
        public void AddItem_ExistingItem_ShouldIncrementQuantityAndUpdateValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: 1, value: 100);
            _draftOrder.AddItem(orderItem);

            // Act
            _draftOrder.AddItem(orderItem);

            // Assert
            _draftOrder.TotalValue.Should().Be(200);
            _draftOrder.OrderItems.Should().HaveCount(1);
            _draftOrder.OrderItems.FirstOrDefault().Quantity.Should().Be(2);
        }
    }
}