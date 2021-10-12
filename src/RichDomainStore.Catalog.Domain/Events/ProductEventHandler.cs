using System.Threading;
using System.Threading.Tasks;
using MediatR;

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
            var product = await _productRepository.GetById(message.AggregateId).ConfigureAwait(false);

            // Notify
        }
    }
}