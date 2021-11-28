using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace RichDomainStore.Catalog.Domain.Events
{
    public class OrderProcessCanceledEventHandler : INotificationHandler<OrderProcessCanceledEvent>
    {
        private readonly IStockService _stockService;

        public OrderProcessCanceledEventHandler(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task Handle(OrderProcessCanceledEvent message, CancellationToken cancellationToken)
        {
            await _stockService.ReStockOrderProductListAsync(message.Products).ConfigureAwait(false);
        }
    }
}