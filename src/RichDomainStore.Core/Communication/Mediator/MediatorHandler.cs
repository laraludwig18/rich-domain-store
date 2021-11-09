using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SendCommandAsync<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task PublishEventAsync<T>(T e) where T : Event
        {
            await _mediator.Publish(e).ConfigureAwait(false);
        }
    }
}