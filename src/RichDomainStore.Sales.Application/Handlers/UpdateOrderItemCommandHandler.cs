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
    public class UpdateOrderItemCommandHandler : CommandHandler, IRequestHandler<UpdateOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateOrderItemCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order not found"))
                    .ConfigureAwait(continueOnCapturedContext: false);

                return false;
            }

            var orderItem = await _orderRepository.GetItemByOrderIdAsync(order.Id, message.ProductId)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (!order.OrderItemExists(orderItem))
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order item not found"))
                    .ConfigureAwait(continueOnCapturedContext: false);

                return false;
            }

            order.UpdateItemQuantity(orderItem, message.Quantity);
            order.AddEvent(new OrderItemUpdatedEvent(message.CustomerId, order.Id, message.ProductId, message.Quantity));

            _orderRepository.UpdateItem(orderItem);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}