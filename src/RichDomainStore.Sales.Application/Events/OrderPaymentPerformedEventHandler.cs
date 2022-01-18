using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderPaymentPerformedEventHandler : INotificationHandler<OrderPaymentPerformedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderPaymentPerformedEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderPaymentPerformedEvent message, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommandAsync(new FinishOrderCommand(message.OrderId, message.CustomerId))
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}