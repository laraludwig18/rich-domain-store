using System;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderUpdatedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal TotalValue { get; private set; }

        public OrderUpdatedEvent(Guid customerId, Guid orderId, decimal totalValue)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            TotalValue = totalValue;
        }
    }
}