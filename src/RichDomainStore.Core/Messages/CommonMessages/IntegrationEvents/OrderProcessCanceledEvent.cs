using System;
using RichDomainStore.Core.DomainObjects.Dtos;

namespace RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class OrderProcessCanceledEvent : IntegrationEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public OrderProductListDto Products { get; private set; }

        public OrderProcessCanceledEvent(Guid orderId, Guid customerId, OrderProductListDto products)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            Products = products;
        }
    }
}