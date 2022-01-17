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

        [Fact]
        public void Constructor_EmptyName_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    string.Empty,
                    "Description",
                    false,
                    100,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product name cannot be empty");
        }

        [Fact]
        public void Constructor_EmptyDescription_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    "Name",
                    string.Empty,
                    false,
                    100,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product description cannot be empty");
        }

        [Fact]
        public void Constructor_ValueLessThanAllowed_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    Product.MinValue - 1,
                    Guid.NewGuid(),
                    "Image",
                    _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage($"Product value cannot be less than {Product.MinValue}");
        }

        [Fact]
        public void Constructor_EmptyCategory_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    100,
                    Guid.Empty,
                    "Image",
                    _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product category id cannot be empty");
        }

        [Fact]
        public void Constructor_EmptyImage_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    "Name",
                    "Description",
                    false,
                    100,
                    Guid.NewGuid(),
                    string.Empty,
                    _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product image cannot be empty");
        }

        [Fact]
        public void Constructor_ValidProduct_ShouldCreateProduct()
        {
            // Arrange & Act
            var product = _productFixture.GenerateValidProduct();

            // Assert
            product.Should().BeOfType<Product>();
        }
    }
}
