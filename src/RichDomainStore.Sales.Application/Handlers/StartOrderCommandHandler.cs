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

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order not found"))
                    .ConfigureAwait(continueOnCapturedContext: false);

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

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        private OrderProductListDto GetOrderProductList(Order order)
        {
            var itemsList = order.OrderItems.Select(i => new ItemDto
            {
                Id = i.ProductId,
                Quantity = i.Quantity
            });

            return new OrderProductListDto
            {
                OrderId = order.Id,
                Items = itemsList
            };
        }
    }
}