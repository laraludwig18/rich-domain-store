using System.Linq;
using System.Threading.Tasks;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Sales.Data.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublishEventsAsync(this IMediatorHandler mediator, SalesContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEventAsync(domainEvent).ConfigureAwait(false);
                });

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}