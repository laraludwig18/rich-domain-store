using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderStockRejectedEventHandler : INotificationHandler<OrderStockRejectedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderStockRejectedEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStockRejectedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommandAsync(new CancelOrderProcessCommand(message.OrderId, message.CustomerId)).ConfigureAwait(false);
        }
    }
}