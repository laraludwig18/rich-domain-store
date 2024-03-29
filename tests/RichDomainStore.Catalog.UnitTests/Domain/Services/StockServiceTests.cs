using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Catalog.Domain.Services;
using RichDomainStore.Catalog.UnitTests.Fixtures;
using RichDomainStore.Core.Communication.Mediator;
using Xunit;

namespace RichDomainStore.Catalog.UnitTests.Domain.Services
{
    [Collection(nameof(ProductCollection))]
    public class StockServiceTests
    {
        private readonly ProductFixture _productFixture;
        private readonly AutoMocker _mocker;
        private readonly StockService _stockService;
        private readonly Product _validProduct;
        public StockServiceTests(ProductFixture productFixture)
        {
            _mocker = new AutoMocker();
            _stockService = _mocker.CreateInstance<StockService>();

            _mocker.GetMock<IProductRepository>()
             .Setup(r => r.UnitOfWork.CommitAsync())
             .ReturnsAsync(true);

            _productFixture = productFixture;
            _validProduct = _productFixture.GenerateValidProduct();
        }

        [Fact]
        public async Task DebitStockAsync_ProductWithoutLowStock_ShouldDebitStock()
        {
            // Arrange
            _validProduct.ReStock(Product.LowStockQuantity + 1);

            _mocker.GetMock<IProductRepository>()
             .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync(_validProduct);

            // Act
            var result = await _stockService.DebitStockAsync(productId: _validProduct.Id, quantity: 1)
                .ConfigureAwait(continueOnCapturedContext: false);

            // Assert
            result.Should().BeTrue();
            _mocker.GetMock<IProductRepository>().Verify(r => r.Update(_validProduct), Times.Once);
            _mocker.GetMock<IProductRepository>().Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DebitStockAsync_ProductWithLowStock_ShouldPublishLowProductStockEvent()
        {
            // Arrange
            _validProduct.ReStock(Product.LowStockQuantity);

            _mocker.GetMock<IProductRepository>()
             .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync(_validProduct);

            // Act
            var result = await _stockService.DebitStockAsync(productId: _validProduct.Id, quantity: 1)
                .ConfigureAwait(continueOnCapturedContext: false);

            // Assert
            result.Should().BeTrue();
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublishDomainEventAsync(It.IsAny<LowProductInStockEvent>()), Times.Once);
        }
    }
}