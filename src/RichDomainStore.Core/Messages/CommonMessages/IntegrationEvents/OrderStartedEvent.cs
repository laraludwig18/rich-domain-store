using System;
using RichDomainStore.Core.DomainObjects.Dtos;

namespace RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class OrderStartedEvent : IntegrationEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Total { get; private set; }
        public OrderProductList Products { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpiration { get; private set; }
        public string CardSecurityCode { get; private set; }

        public OrderStartedEvent(Guid orderId,
            Guid customerId,
            decimal total,
            OrderProductList products,
            string cardName,
            string cardNumber,
            string cardExpiration,
            string cardSecurityCode)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
            Products = products;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardSecurityCode = cardSecurityCode;
        }
    }
}