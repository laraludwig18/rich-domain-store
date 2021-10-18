using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Catalog.Domain.Interfaces;

namespace RichDomainStore.Catalog.Domain.Events
{
    public class ProductEventHandler : INotificationHandler<LowProductInStockEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductEventHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(LowProductInStockEvent message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(message.AggregateId).ConfigureAwait(false);

            // Notify
        }
    }
}