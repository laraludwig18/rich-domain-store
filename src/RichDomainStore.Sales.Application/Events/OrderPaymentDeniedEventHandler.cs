using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderPaymentDeniedEventHandler : INotificationHandler<OrderPaymentDeniedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderPaymentDeniedEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderPaymentDeniedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommandAsync(new CancelOrderProcessReversingStockCommand(message.OrderId, message.CustomerId))
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}