using System;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.Communication.Mediator;

namespace RichDomainStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediator;

        public StockService(IProductRepository productRepository, IMediatorHandler mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<bool> DebitStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);
            if (product == null) 
            {
                return false;
            }

            if (!product.HasStock(quantity)) 
            {
                return false;
            }

            product.DebitStock(quantity);

            if (product.StockQuantity < 10)
            {
                await _mediator.PublishEventAsync(new LowProductInStockEvent(product.Id, product.StockQuantity)).ConfigureAwait(false);
            }

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<bool> ReStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);
            if (product == null) 
            {
                return false;
            }

            product.ReStock(quantity);

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}