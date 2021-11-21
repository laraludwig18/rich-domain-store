using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Events;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class AddOrderItemCommandHandler : CommandHandler, IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AddOrderItemCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId).ConfigureAwait(false);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Quantity, message.Value);

            order = order == null
                ? CreateNewDraftOrder(message.CustomerId, orderItem)
                : AddItemToExistentOrder(order, orderItem);

            order.AddEvent(
                new OrderItemAddedEvent(message.CustomerId,
                    order.Id,
                    message.ProductId,
                    message.ProductName,
                    message.Value,
                    message.Quantity));

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        private Order CreateNewDraftOrder(Guid customerId, OrderItem orderItem)
        {
            var order = Order.OrderFactory.NewDraft(customerId);
            order.AddItem(orderItem);

            _orderRepository.Add(order);
            order.AddEvent(new DraftOrderStartedEvent(customerId, order.Id));

            return order;
        }

        private Order AddItemToExistentOrder(Order order, OrderItem orderItem)
        {
            var orderItemExistent = order.CheckOrderItemExists(orderItem);
            order.AddItem(orderItem);

            if (orderItemExistent)
            {
                _orderRepository.UpdateItem(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
            }
            else
            {
                _orderRepository.AddItem(orderItem);
            }

            order.AddEvent(new OrderUpdatedEvent(order.CustomerId, order.Id, order.TotalValue));

            return order;
        }
    }
}