using System;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.DomainObjects.Dtos;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;

namespace RichDomainStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public StockService(IProductRepository productRepository, IMediatorHandler mediatorHandler)
        {
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> DebitStockAsync(Guid productId, int quantity)
        {
            var success = await DebitStockItemAsync(productId, quantity).ConfigureAwait(false);

            if (!success)
            {
                return false;
            }

            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<bool> DebitOrderProductListAsync(OrderProductListDto list)
        {
            foreach (var item in list.Items)
            {
                if (!await DebitStockItemAsync(item.Id, item.Quantity).ConfigureAwait(false))
                {
                    return false;
                }
            }

            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        private async Task<bool> DebitStockItemAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);
            if (product == null)
            {
                return false;
            }

            if (!product.HasStock(quantity))
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("Stock", $"Product - {product.Name} is out of stock"));
                return false;
            }

            product.DebitStock(quantity);

            if (product.IsLowStock())
            {
                await _mediatorHandler.PublishDomainEventAsync(new LowProductInStockEvent(product.Id, product.StockQuantity)).ConfigureAwait(false);
            }

            _productRepository.Update(product);
            return true;
        }

        public async Task<bool> ReStockAsync(Guid productId, int quantity)
        {
            var success = await ReStockItemAsync(productId, quantity).ConfigureAwait(false);

            if (!success)
            {
                return false;
            }

            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        public async Task<bool> ReStockOrderProductListAsync(OrderProductListDto list)
        {
            foreach (var item in list.Items)
            {
                if (!await ReStockItemAsync(item.Id, item.Quantity).ConfigureAwait(false))
                {
                    return false;
                }
            }

            return await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        private async Task<bool> ReStockItemAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId).ConfigureAwait(false);
            if (product == null)
            {
                return false;
            }

            product.ReStock(quantity);

            _productRepository.Update(product);

            return true;
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}