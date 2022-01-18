using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Core.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublishEventsAsync(this IMediatorHandler mediator, DbContext ctx)
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
                    await mediator.PublishEventAsync(domainEvent).ConfigureAwait(continueOnCapturedContext: false);
                });

            await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}