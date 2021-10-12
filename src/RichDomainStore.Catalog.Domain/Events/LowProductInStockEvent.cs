using System;
using RichDomainStore.Core.DomainObjects;

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