using System;

namespace RichDomainStore.Core.Messages.CommonMessages.DomainEvents
{
    public abstract class DomainEvent : Event
    {
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}