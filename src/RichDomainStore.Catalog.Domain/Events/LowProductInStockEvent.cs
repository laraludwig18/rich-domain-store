using System;
using RichDomainStore.Core.Messages.CommonMessages.DomainEvents;

namespace RichDomainStore.Catalog.Domain.Events
{
    public class LowProductInStockEvent : DomainEvent
    {
        public int RemainingQuantity { get; private set; }

        public LowProductInStockEvent(Guid aggregateId, int remainingQuantity) : base(aggregateId)
        {
            RemainingQuantity = remainingQuantity;
        }
    }
}