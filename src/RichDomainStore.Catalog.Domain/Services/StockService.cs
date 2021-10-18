using System;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.Bus;

namespace RichDomainStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatrHandler _bus;

        public StockService(IProductRepository productRepository, IMediatrHandler bus)
        {
            _productRepository = productRepository;
            _bus = bus;
        }

        public async Task<bool> DebitStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);

            if (product == null) return false;

            if (!product.HasStock(quantity)) return false;

            product.DebitStock(quantity);

            if (product.StockQuantity < 10)
            {
                await _bus.PublishEvent(new LowProductInStockEvent(product.Id, product.StockQuantity));
            }

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task<bool> ReStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);

            if (product == null) return false;

            product.ReStock(quantity);

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.Commit().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}