using System;
using System.Linq;
using FluentAssertions;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Enums;
using RichDomainStore.Sales.UnitTests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Domain.Entities
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

        [Fact]
        public void ApplyVoucher_ValidVoucher_ShouldNotReturnErrors()
        {
            // Arrange
            var voucher = _orderFixture.GenerateValidVoucher(discountType: VoucherDiscountType.Value, discountValue: 10);

            // Act
            var result = _draftOrder.ApplyVoucher(voucher);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ApplyVoucher_InvalidVoucher_ShouldReturnErrors()
        {
            // Arrange
            var voucher = _orderFixture.GenerateInvalidVoucher(discountType: VoucherDiscountType.Value);

            // Act
            var result = _draftOrder.ApplyVoucher(voucher);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ApplyVoucher_ValueDiscountType_ShouldDiscountTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 200);
            _draftOrder.AddItem(orderItem);

            var voucher = _orderFixture.GenerateValidVoucher(discountType: VoucherDiscountType.Value, discountValue: 10);

            var expectedTotalValue = _draftOrder.TotalValue - voucher.DiscountValue;

            // Act
            _draftOrder.ApplyVoucher(voucher);

            // Assert
            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }

        [Fact]
        public void ApplyVoucher_PercentageDiscountType_ShouldDiscountTotalValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 200);
            _draftOrder.AddItem(orderItem);

            var voucher = _orderFixture.GenerateValidVoucher(
                discountType: VoucherDiscountType.Percentage,
                discountPercentage: 10);

            var discountValue = (_draftOrder.TotalValue * voucher.DiscountPercentage) / 100;
            var expectedTotalValue = _draftOrder.TotalValue - discountValue;

            // Act
            _draftOrder.ApplyVoucher(voucher);

            // Assert
            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }

        [Fact]
        public void ApplyVoucher_DiscountGreatherThanTotalValue_ShouldSetTotalValueToZero()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 10);
            _draftOrder.AddItem(orderItem);

            var voucher = _orderFixture.GenerateValidVoucher(discountType: VoucherDiscountType.Value, discountValue: 15);

            // Act
            _draftOrder.ApplyVoucher(voucher);

            // Assert
            _draftOrder.TotalValue.Should().Be(0);
        }

        [Fact]
        public void AddItem_NewItemWithVoucherApplied_ShouldUpdateDiscountValue()
        {
            // Arrange
            var orderItem = _orderFixture.GenerateValidOrderItem(value: 200);
            _draftOrder.AddItem(orderItem);

            var voucher = _orderFixture.GenerateValidVoucher(
                discountType: VoucherDiscountType.Percentage,
                discountPercentage: 15);

            _draftOrder.ApplyVoucher(voucher);

            var newOrderItem = _orderFixture.GenerateValidOrderItem(value: 100);

            // Act
            _draftOrder.AddItem(newOrderItem);

            // Assert
            var totalValue = _draftOrder.OrderItems.Sum(i => i.Quantity * i.Value);
            var discountValue = (totalValue * voucher.DiscountPercentage) / 100;
            var expectedTotalValue = totalValue - discountValue;

            _draftOrder.TotalValue.Should().Be(expectedTotalValue);
        }
    }
}