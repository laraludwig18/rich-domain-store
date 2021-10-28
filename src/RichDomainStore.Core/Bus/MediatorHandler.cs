using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Bus
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T e) where T : Event
        {
            await _mediator.Publish(e).ConfigureAwait(false);
        }
    }
}