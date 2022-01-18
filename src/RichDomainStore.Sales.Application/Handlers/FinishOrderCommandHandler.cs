using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Events;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class FinishOrderCommandHandler : CommandHandler, IRequestHandler<FinishOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public FinishOrderCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(FinishOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetByIdAsync(message.OrderId).ConfigureAwait(continueOnCapturedContext: false);
            if (order == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order not found"))
                    .ConfigureAwait(continueOnCapturedContext: false);

                return false;
            }

            order.FinishOrder();

            order.AddEvent(new OrderFinishedEvent(message.OrderId));

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}