using System;
using RichDomainStore.Core.DomainObjects;
using Xunit;

namespace RichDomainStore.Catalog.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Validate_EmptyProductNameShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product(string.Empty, "Description", false, 100, Guid.NewGuid(), DateTime.Now, "Image", new Dimensions(1, 1, 1))
            );

            Assert.Equal("Product name cannot be empty", ex.Message);
        }
        
        [Fact]
        public void Product_Validate_EmptyProductDescriptionShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product("Name", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, "Image", new Dimensions(1, 1, 1))
            );

            Assert.Equal("Product description cannot be empty", ex.Message);
        }
        
        [Fact]
        public void Product_Validate_ProductValueLessThan1ShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product("Name", "Description", false, 0, Guid.NewGuid(), DateTime.Now, "Image", new Dimensions(1, 1, 1))
            );

            Assert.Equal("Product value cannot be less than 1", ex.Message);
        }

        [Fact]
        public void Product_Validate_EmptyProductCategoryShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product("Name", "Description", false, 100, Guid.Empty, DateTime.Now, "Image", new Dimensions(1, 1, 1))
            );

            Assert.Equal("Product category id cannot be empty", ex.Message);
        }

        [Fact]
        public void Product_Validate_EmptyProductImageShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product("Name", "Description", false, 100, Guid.NewGuid(), DateTime.Now, string.Empty, new Dimensions(1, 1, 1))
            );

            Assert.Equal("Product image cannot be empty", ex.Message);
        }

        [Fact]
        public void Product_Validate_HeightLessThan1ShouldThrowException()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Product("Name", "Description", false, 100, Guid.NewGuid(), DateTime.Now, "Image", new Dimensions(0, 1, 1))
            );

            Assert.Equal("Height cannot be less than 1", ex.Message);
        }
    }
}
