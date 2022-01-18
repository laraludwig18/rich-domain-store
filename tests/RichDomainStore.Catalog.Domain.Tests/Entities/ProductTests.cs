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
            _validDimensions = new Dimensions(height: 1, width: 1, depth: 1);
            _productFixture = productFixture;
        }

        [Fact]
        public void Constructor_EmptyName_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    name: string.Empty,
                    description: "Description",
                    active: false,
                    value: 100,
                    categoryId: Guid.NewGuid(),
                    image: "Image",
                    dimensions: _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product name cannot be empty");
        }

        [Fact]
        public void Constructor_EmptyDescription_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    name: "Name",
                    description: string.Empty,
                    active: false,
                    value: 100,
                    categoryId: Guid.NewGuid(),
                    image: "Image",
                    dimensions: _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product description cannot be empty");
        }

        [Fact]
        public void Constructor_ValueLessThanAllowed_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    name: "Name",
                    description: "Description",
                    active: false,
                    value: Product.MinValue - 1,
                    categoryId: Guid.NewGuid(),
                    image: "Image",
                    dimensions: _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage($"Product value cannot be less than {Product.MinValue}");
        }

        [Fact]
        public void Constructor_EmptyCategory_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    name: "Name",
                    description: "Description",
                    active: false,
                    value: 100,
                    categoryId: Guid.Empty,
                    image: "Image",
                    dimensions: _validDimensions);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product category id cannot be empty");
        }

        [Fact]
        public void Constructor_EmptyImage_ShouldThrowException()
        {
            // Arrange & Act
            Action act = () =>
                new Product(
                    name: "Name",
                    description: "Description",
                    active: false,
                    value: 100,
                    categoryId: Guid.NewGuid(),
                    image: string.Empty,
                    dimensions: _validDimensions);

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
