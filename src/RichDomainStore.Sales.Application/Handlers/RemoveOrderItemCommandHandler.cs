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
    public class RemoveOrderItemCommandHandler : CommandHandler, IRequestHandler<RemoveOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public RemoveOrderItemCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId).ConfigureAwait(false);
            if (order == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order not found")).ConfigureAwait(false);
                return false;
            }

            var orderItem = await _orderRepository.GetItemByOrderIdAsync(order.Id, message.ProductId).ConfigureAwait(false);
            if (!order.CheckOrderItemExists(orderItem))
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order item not found")).ConfigureAwait(false);
                return false;
            }

            order.RemoveItem(orderItem);
            order.AddEvent(new OrderItemRemovedEvent(message.CustomerId, order.Id, message.ProductId));

            _orderRepository.RemoveItem(orderItem);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}