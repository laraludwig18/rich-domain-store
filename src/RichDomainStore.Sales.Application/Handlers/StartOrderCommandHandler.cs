using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.DomainObjects.Dtos;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class StartOrderCommandHandler : CommandHandler, IRequestHandler<StartOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public StartOrderCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(StartOrderCommand message, CancellationToken cancellationToken)
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

            order.StartOrder();

            var orderProductList = GetOrderProductList(order);

            order.AddEvent(
                    new OrderStartedEvent(order.Id,
                        order.CustomerId,
                        order.TotalValue,
                        orderProductList,
                        message.CardName,
                        message.CardNumber,
                        message.CardExpiration,
                        message.CardSecurityCode));

            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }

        private OrderProductList GetOrderProductList(Order order)
        {
            var itemsList = order.OrderItems.Select(i => new Item
            {
                Id = i.ProductId,
                Quantity = i.Quantity
            });

            return new OrderProductList
            {
                OrderId = order.Id,
                Items = itemsList
            };
        }
    }
}