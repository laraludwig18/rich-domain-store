using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AddOrderItemCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler)
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

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Quantity, message.Value);

            order = order == null
                ? CreateNewDraftOrder(message.CustomerId, orderItem)
                : AddItemToExistentOrder(order, orderItem);

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        private Order CreateNewDraftOrder(Guid customerId, OrderItem orderItem)
        {
            var order = Order.OrderFactory.NewDraft(customerId);
            order.AddItem(orderItem);

            _orderRepository.AddAsync(order);

            return order;
        }

        private Order AddItemToExistentOrder(Order order, OrderItem orderItem)
        {
            var orderItemExistent = order.CheckOrderItemExists(orderItem);
            order.AddItem(orderItem);

            if (orderItemExistent)
            {
                _orderRepository.UpdateItemAsync(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
            }
            else
            {
                _orderRepository.AddItemAsync(orderItem);
            }

            return order;
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid())
            {
                return true;
            }

            foreach (var error in message.ValidationResult.Errors)
            {
                // _mediatorHandler.PublishEventAsync(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}