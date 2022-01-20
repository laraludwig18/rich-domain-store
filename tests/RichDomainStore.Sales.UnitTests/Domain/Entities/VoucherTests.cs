using FluentAssertions;
using RichDomainStore.Sales.Domain.Enums;
using RichDomainStore.Sales.UnitTests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Domain.Entities
{
    [Collection(nameof(OrderCollection))]
    public class VoucherTests
    {
        private readonly OrderFixture _orderFixture;
        public VoucherTests(OrderFixture orderFixture)
        {
            _orderFixture = orderFixture;
        }

        [Fact]
        public void ValidateIfApplicable_ValueDiscountType_ShouldBeValid()
        {
            // Arrange
            var voucher = _orderFixture.GenerateValidVoucher(discountType: VoucherDiscountType.Value, discountValue: 10);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfApplicable_ValueDiscountType_ShouldBeInvalid()
        {
            // Arrange
            var voucher = _orderFixture.GenerateInvalidVoucher(discountType: VoucherDiscountType.Value);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(6).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("Voucher code cannot be empty"),
                second => second.ErrorMessage.Should().Be("Voucher is expired"),
                third => third.ErrorMessage.Should().Be("Voucher is not valid"),
                fourth => fourth.ErrorMessage.Should().Be("Voucher has already been used"),
                fifth => fifth.ErrorMessage.Should().Be("Voucher not available"),
                sixth => sixth.ErrorMessage.Should().Be("Voucher discount value must be greather than 0"));
        }

        [Fact]
        public void ValidateIfApplicable_PercentageDiscountType_ShouldBeValid()
        {
            // Arrange
            var voucher = _orderFixture.GenerateValidVoucher(
                discountType: VoucherDiscountType.Percentage,
                discountPercentage: 10);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfApplicable_PercentageDiscountType_ShouldBeInvalid()
        {
            // Arrange
            var voucher = _orderFixture.GenerateInvalidVoucher(discountType: VoucherDiscountType.Percentage);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(6).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("Voucher code cannot be empty"),
                second => second.ErrorMessage.Should().Be("Voucher is expired"),
                third => third.ErrorMessage.Should().Be("Voucher is not valid"),
                fourth => fourth.ErrorMessage.Should().Be("Voucher has already been used"),
                fifth => fifth.ErrorMessage.Should().Be("Voucher not available"),
                sixth => sixth.ErrorMessage.Should().Be("Voucher discount percentage must be greather than 0"));
        }
    }
}