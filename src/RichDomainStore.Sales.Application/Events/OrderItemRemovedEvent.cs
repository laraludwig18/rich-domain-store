using System;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderItemRemovedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public OrderItemRemovedEvent(Guid customerId, Guid orderId, Guid productId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }
    }
}