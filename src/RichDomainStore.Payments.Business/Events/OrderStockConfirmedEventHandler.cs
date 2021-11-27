using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.DomainObjects.Dtos;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Payments.Business.Interfaces;

namespace RichDomainStore.Payments.Business.Events
{
    public class OrderStockConfirmedEventHandler : INotificationHandler<OrderStockConfirmedEvent>
    {
        private readonly IPaymentService _paymentService;

        public OrderStockConfirmedEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(OrderStockConfirmedEvent message, CancellationToken cancellationToken)
        {
            var orderPayment = new OrderPaymentDto
            {
                OrderId = message.OrderId,
                CustomerId = message.CustomerId,
                Total = message.Total,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                CardExpiration = message.CardExpiration,
                CardSecurityCode = message.CardSecurityCode
            };

            await _paymentService.PerformOrderPayment(orderPayment).ConfigureAwait(false);
        }
    }
}