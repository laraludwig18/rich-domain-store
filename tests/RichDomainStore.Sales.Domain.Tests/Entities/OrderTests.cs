using System;
using System.Linq;
using FluentAssertions;
using RichDomainStore.Sales.Domain.Entities;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Entities
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            order.TotalValue.Should().Be(200);
        }

        [Fact]
        public void AddItem_ExistingItem_ShouldIncrementQuantityAndUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", 1, 100);
            order.AddItem(orderItem);

            // Act
            order.AddItem(orderItem);

            // Assert
            order.TotalValue.Should().Be(200);
            order.OrderItems.Should().HaveCount(1);
            order.OrderItems.FirstOrDefault().Quantity.Should().Be(2);
        }
    }
}