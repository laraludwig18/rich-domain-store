using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace RichDomainStore.Catalog.Domain.Events
{
    public class OrderStartedEventHandler : INotificationHandler<OrderStartedEvent>
    {
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public OrderStartedEventHandler(IStockService stockService, IMediatorHandler mediatorHandler)
        {
            _stockService = stockService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken)
        {
            var success = await _stockService.DebitOrderProductListAsync(message.Products).ConfigureAwait(false);

            if (success)
            {
                await _mediatorHandler.PublishEventAsync(
                    new OrderStockConfirmedEvent(message.OrderId,
                        message.CustomerId,
                        message.Total,
                        message.Products,
                        message.CardName,
                        message.CardNumber,
                        message.CardExpiration,
                        message.CardSecurityCode)).ConfigureAwait(false);
            }
            else
            {
                await _mediatorHandler.PublishEventAsync(
                    new OrderStockRejectedEvent(message.OrderId, message.CustomerId)).ConfigureAwait(false);
            }
        }
    }
}