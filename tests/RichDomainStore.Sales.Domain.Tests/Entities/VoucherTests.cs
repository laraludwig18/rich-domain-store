using System;
using FluentAssertions;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Enums;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Entities
{
    public class VoucherTests
    {
        [Fact]
        public void ValidateIfApplicable_ValueDiscountType_ShouldBeValid()
        {
            // Arrange
            var voucher = new Voucher(
                code: "PROMO-15-REAIS",
                discountValue: 15,
                quantity: 1,
                discountType: VoucherDiscountType.Value,
                expirationDate: DateTime.Now.AddDays(10),
                active: true,
                used: false);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfApplicable_ValueDiscountType_ShouldBeInvalid()
        {
            // Arrange
            var voucher = new Voucher(
                code: string.Empty,
                quantity: 0,
                discountType: VoucherDiscountType.Value,
                expirationDate: DateTime.Now.AddDays(-1),
                active: false,
                used: true);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(6).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("Voucher code cannot be empty."),
                second => second.ErrorMessage.Should().Be("Voucher is expired."),
                third => third.ErrorMessage.Should().Be("Voucher is not valid."),
                fourth => fourth.ErrorMessage.Should().Be("Voucher has already been used."),
                fifth => fifth.ErrorMessage.Should().Be("Voucher not available."),
                sixth => sixth.ErrorMessage.Should().Be("Voucher discount value must be greather than 0."));
        }

        [Fact]
        public void ValidateIfApplicable_PercentageDiscountType_ShouldBeValid()
        {
            // Arrange
            var voucher = new Voucher(
                code: "PROMO-10-OFF",
                discountPercentage: 10,
                quantity: 1,
                discountType: VoucherDiscountType.Percentage,
                expirationDate: DateTime.Now.AddDays(10),
                active: true,
                used: false);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfApplicable_PercentageDiscountType_ShouldBeInvalid()
        {
            // Arrange
            var voucher = new Voucher(
                code: string.Empty,
                quantity: 0,
                discountType: VoucherDiscountType.Percentage,
                expirationDate: DateTime.Now.AddDays(-1),
                active: false,
                used: true);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(6).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("Voucher code cannot be empty."),
                second => second.ErrorMessage.Should().Be("Voucher is expired."),
                third => third.ErrorMessage.Should().Be("Voucher is not valid."),
                fourth => fourth.ErrorMessage.Should().Be("Voucher has already been used."),
                fifth => fifth.ErrorMessage.Should().Be("Voucher not available."),
                sixth => sixth.ErrorMessage.Should().Be("Voucher discount percentage must be greather than 0."));
        }
    }
}