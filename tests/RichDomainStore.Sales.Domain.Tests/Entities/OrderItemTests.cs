using System;
using FluentAssertions;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Entities;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void Constructor_QuantityGreatherThanAllowed_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new OrderItem(Guid.NewGuid(), "Product Name", OrderItem.MaxItemQuantity + 1, 100);

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
                new OrderItem(Guid.NewGuid(), "Product Name", OrderItem.MinItemQuantity - 1, 100);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }

        [Fact]
        public void IncrementQuantity_SumOfQuantityGreatherThanAllowed_ShouldThrowException()
        {
            // Arrange 
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", OrderItem.MaxItemQuantity, 100);

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
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", OrderItem.MaxItemQuantity, 100);

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
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", OrderItem.MinItemQuantity, 100);

            // Act
            Action act = () => orderItem.UpdateQuantity(OrderItem.MinItemQuantity - 1);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage($"Product quantity cannot be less than {OrderItem.MinItemQuantity} or greather than {OrderItem.MaxItemQuantity}");
        }
    }
}