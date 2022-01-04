using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using RichDomainStore.Core.Data.EventSourcing;
using RichDomainStore.Core.Messages;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task SaveEventAsync<TEvent>(TEvent e) where TEvent : Event
        {
            await _eventStoreService.GetConnection()
                .AppendToStreamAsync(
                    e.AggregateId.ToString(),
                    ExpectedVersion.Any,
                    FormatEvent(e))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<StoredEvent>> GetEventsByAggregateIdAsync(Guid aggregateId)
        {
            var events = await _eventStoreService.GetConnection()
                .ReadStreamEventsForwardAsync(aggregateId.ToString(), 0, 500, false)
                .ConfigureAwait(false);

            var eventList = events.Events.Select(resolvedEvent => {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var jsonData = JsonConvert.DeserializeObject<BaseEvent>(dataEncoded);

                return new StoredEvent(
                    resolvedEvent.Event.EventId,
                    resolvedEvent.Event.EventType,
                    jsonData.Timestamp,
                    dataEncoded);
            });

            return eventList.OrderBy(e => e.OccurrenceDate);
        }

        private static IEnumerable<EventData> FormatEvent<TEvent>(TEvent e) where TEvent : Event
        {
            yield return new EventData(
                Guid.NewGuid(),
                e.MessageType,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                null);
        }
    }
}