using System;
using FluentAssertions;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.Tests.Fixtures;
using RichDomainStore.Core.DomainObjects;
using Xunit;

namespace RichDomainStore.Catalog.Domain.Tests.Entities
{
    [Collection(nameof(ProductCollection))]
    public class ProductTests
    {
        private readonly Dimensions _validDimensions;
        private readonly ProductFixture _productFixture;
        public ProductTests(ProductFixture productFixture)
        {
            _validDimensions = new Dimensions(1, 1, 1);
            _productFixture = productFixture;
        }

        [Fact(DisplayName = "Invalid Name")]
        public void Validate_EmptyName_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    string.Empty,
                    "Description",
                    false,
                    100,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions)
            );

            ex.Message.Should().Be("Product name cannot be empty");
        }

        [Fact(DisplayName = "Invalid Description")]
        public void Validate_EmptyDescription_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    "Name",
                    string.Empty,
                    false,
                    100,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions)
            );

            ex.Message.Should().Be("Product description cannot be empty");
        }

        [Fact(DisplayName = "Invalid Value")]
        public void Validate_ValueLessThan1_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    0,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions)
            );

            ex.Message.Should().Be("Product value cannot be less than 1");
        }

        [Fact(DisplayName = "Invalid Category")]
        public void Validate_EmptyCategory_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    100,
                    Guid.Empty,
                    "Image",
                    _validDimensions)
            );

            ex.Message.Should().Be("Product category id cannot be empty");
        }

        [Fact(DisplayName = "Invalid Image")]
        public void Validate_EmptyImage_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    100,
                    Guid.NewGuid(),
                    string.Empty,
                    _validDimensions)
            );

            ex.Message.Should().Be("Product image cannot be empty");
        }

        [Fact(DisplayName = "Invalid Height")]
        public void Validate_HeightLessThan1_ShouldThrowException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    100,
                    Guid.NewGuid(),
                    "Image",
                    new Dimensions(0, 1, 1))
            );

            ex.Message.Should().Be("Height cannot be less than 1");
        }

        [Fact(DisplayName = "Valid Product")]
        public void Validate_ValidProduct_ShouldCreateProduct()
        {
            // Arrange & Act
            var product = _productFixture.GenerateValidProduct();

            // Assert
            product.Should().BeOfType<Product>();
        }
    }
}
