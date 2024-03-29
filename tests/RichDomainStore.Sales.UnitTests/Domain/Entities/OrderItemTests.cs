using System;
using FluentAssertions;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.UnitTests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Domain.Entities
{
    [Collection(nameof(OrderCollection))]
    public class OrderItemTests
    {
        private readonly OrderFixture _orderFixture;
        public OrderItemTests(OrderFixture orderFixture)
        {
            _orderFixture = orderFixture;
        }

        [Fact]
        public void Constructor_QuantityGreatherThanAllowed_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new OrderItem(
                    productId: Guid.NewGuid(),
                    productName: "Product Name",
                    quantity: OrderItem.MaxItemQuantity + 1,
                    value: 100);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }

        [Fact]
        public void Constructor_QuantityLessThanAllowed_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new OrderItem(
                    productId: Guid.NewGuid(),
                    productName: "Product Name",
                    quantity: OrderItem.MinItemQuantity - 1,
                    value: 100);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }

        [Fact]
        public void IncrementQuantity_SumOfQuantityGreatherThanAllowed_ShouldThrowException()
        {
            // Arrange 
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: OrderItem.MaxItemQuantity);

            // Act
            Action act = () => orderItem.IncrementQuantity(1);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }

        [Fact]
        public void UpdateQuantity_QuantityGreatherThanAllowed_ShouldThrowException()
        {
            // Arrange 
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: OrderItem.MaxItemQuantity);

            // Act
            Action act = () => orderItem.UpdateQuantity(OrderItem.MaxItemQuantity + 1);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }

        [Fact]
        public void UpdateQuantity_QuantityLessThanAllowed_ShouldThrowException()
        {
            // Arrange 
            var orderItem = _orderFixture.GenerateValidOrderItem(itemQuantity: OrderItem.MinItemQuantity);

            // Act
            Action act = () => orderItem.UpdateQuantity(OrderItem.MinItemQuantity - 1);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }
    }
}