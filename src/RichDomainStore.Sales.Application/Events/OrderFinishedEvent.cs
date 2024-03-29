using System;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderFinishedEvent : Event
    {
        public Guid OrderId { get; private set; }

        public OrderFinishedEvent(Guid orderId)
        {
            OrderId = orderId;
            AggregateId = orderId;
        }
    }
}