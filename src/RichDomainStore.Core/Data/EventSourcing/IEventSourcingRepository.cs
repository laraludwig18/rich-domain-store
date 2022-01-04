using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Data.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SaveEventAsync<TEvent>(TEvent e) where TEvent : Event;
        Task<IEnumerable<StoredEvent>> GetEventsByAggregateIdAsync(Guid aggregateId);
    }
}