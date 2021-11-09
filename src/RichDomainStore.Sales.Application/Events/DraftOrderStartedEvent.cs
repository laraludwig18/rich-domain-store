using System;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Sales.Application.Events
{
    public class DraftOrderStartedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public DraftOrderStartedEvent(Guid customerId, Guid orderId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}