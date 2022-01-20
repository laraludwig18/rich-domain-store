using FluentAssertions;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.UnitTests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Application.Commands
{
    [Collection(nameof(AddOrderItemCommandCollection))]
    public class AddOrderItemCommandTests
    {
        private readonly AddOrderItemCommandFixture _addOrderItemCommandFixture;
        public AddOrderItemCommandTests(AddOrderItemCommandFixture addOrderItemCommandFixture)
        {
            _addOrderItemCommandFixture = addOrderItemCommandFixture;
        }

        [Fact]
        public void IsValid_ValidCommand_ShouldBeValid()
        {
            // Arrange
            var command = _addOrderItemCommandFixture.GenerateValidCommand();

            // Act
            var result = command.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InvalidCommandQuantityLessThanAllowed_ShouldBeInvalid()
        {
            // Arrange
            var command = _addOrderItemCommandFixture.GenerateInvalidCommand(itemQuantity: OrderItem.MinItemQuantity - 1);

            // Act
            var result = command.IsValid();

            // Assert
            result.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(5).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("CustomerId is invalid"),
                second => second.ErrorMessage.Should().Be("ProductId is invalid"),
                third => third.ErrorMessage.Should().Be("ProductName is required"),
                fourth => fourth.ErrorMessage.Should().Be($"The minimum quantity of an item is {OrderItem.MinItemQuantity}"),
                sixth => sixth.ErrorMessage.Should().Be("Value must be greather than 0"));
        }

        [Fact]
        public void IsValid_InvalidCommandQuantityGreatherThanAllowed_ShouldBeInvalid()
        {
            // Arrange
            var command = _addOrderItemCommandFixture.GenerateInvalidCommand(itemQuantity: OrderItem.MaxItemQuantity + 1);

            // Act
            var result = command.IsValid();

            // Assert
            result.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(5).And.SatisfyRespectively(
                first => first.ErrorMessage.Should().Be("CustomerId is invalid"),
                second => second.ErrorMessage.Should().Be("ProductId is invalid"),
                third => third.ErrorMessage.Should().Be("ProductName is required"),
                fourth => fourth.ErrorMessage.Should().Be($"The maximum quantity of an item is {OrderItem.MaxItemQuantity}"),
                sixth => sixth.ErrorMessage.Should().Be("Value must be greather than 0"));
        }
    }
}