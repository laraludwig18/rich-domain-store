using System;
using System.Linq;
using FluentAssertions;
using RichDomainStore.Core.DomainObjects;
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
        private readonly Guid _productId;
        public OrderTests(OrderFixture orderFixture)
        {
            _orderFixture = orderFixture;
            _draftOrder = _orderFixture.GenerateDraftOrder();
            _productId = Guid.NewGuid();
        }

        [Fact]
        public void AddItem_NewOrder_ShouldUpdateTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: 2, value: 100);

            var expectedTotalValue = orderItem.Quantity * orderItem.Value;

            // Act
            _draftOrder.AddItem(orderItem);

            // Assert
            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }

        [Fact]
        public void AddItem_ExistingItem_ShouldIncrementQuantityAndUpdateTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 100);
            _draftOrder.AddItem(orderItem);

            // Act
            _draftOrder.AddItem(orderItem);

            // Assert
            _draftOrder.TotalValue.Should().Be(200);
            _draftOrder.OrderItems.Should().HaveCount(1);
            _draftOrder.OrderItems.FirstOrDefault().Quantity.Should().Be(2);
        }

        [Fact]
        public void UpdateItem_NonExistingItem_ShouldThrowException()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem();

            // Act
            Action act = () => _draftOrder.UpdateItem(orderItem);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage("Item does not exists in the order");
        }

        [Fact]
        public void UpdateItem_ExistingItem_ShouldUpdateQuantity()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(_productId);
            _draftOrder.AddItem(orderItem);
            var updatedItem = _orderFixture.GenerateValidOrderItem(_productId, itemQuantity: 2);

            // Act
            _draftOrder.UpdateItem(updatedItem);

            // Assert
            _draftOrder.OrderItems.FirstOrDefault().Quantity.Should().Be(updatedItem.Quantity);
        }

        [Fact]
        public void UpdateItem_OrderWithDifferentItems_ShouldUpdateTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 100);
            var orderItem2 = _orderFixture.GenerateValidOrderItem(_productId);

            _draftOrder.AddItem(orderItem);
            _draftOrder.AddItem(orderItem2);

            var updatedItem = _orderFixture.GenerateValidOrderItem(_productId, itemQuantity: 2, value: 200);
            var expectedTotalValue = orderItem.Quantity * orderItem.Value + updatedItem.Quantity * updatedItem.Value;

            // Act
            _draftOrder.UpdateItem(updatedItem);

            // Assert
            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }

        [Fact]
        public void RemoveItem_NonExistingItem_ShouldThrowException()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem();

            // Act
            Action act = () => _draftOrder.RemoveItem(orderItem);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage("Item does not exists in the order");
        }

        [Fact]
        public void RemoveItem_ExistingItem_ShouldUpdateTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 200);
            var orderItem2 = _orderFixture.GenerateValidOrderItem(value: 300);
            _draftOrder.AddItem(orderItem);
            _draftOrder.AddItem(orderItem2);

            var expectedTotalValue = orderItem.Quantity * orderItem.Value;

            // Act
            _draftOrder.RemoveItem(orderItem2);

            // Assert
            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }
    }
}