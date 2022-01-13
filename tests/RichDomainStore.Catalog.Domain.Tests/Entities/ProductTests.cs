using System;
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

            Assert.Equal("Product name cannot be empty", ex.Message);
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

            Assert.Equal("Product description cannot be empty", ex.Message);
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

            Assert.Equal("Product value cannot be less than 1", ex.Message);
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

            Assert.Equal("Product category id cannot be empty", ex.Message);
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

            Assert.Equal("Product image cannot be empty", ex.Message);
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

            Assert.Equal("Height cannot be less than 1", ex.Message);
        }

        [Fact(DisplayName = "Valid Product")]
        public void Validate_ValidProduct_ShouldCreateProduct()
        {
            // Arrange & Act
            var product = _productFixture.GenerateValidProduct();

            // Assert
            Assert.IsType<Product>(product);
        }
    }
}
