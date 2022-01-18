using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Data.EventSourcing;
using RichDomainStore.Core.Messages;
using RichDomainStore.Core.Messages.CommonMessages.DomainEvents;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;

namespace RichDomainStore.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator, IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> SendCommandAsync<T>(T command) where T : Command
        {
            return await _mediator.Send(command).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task PublishEventAsync<T>(T e) where T : Event
        {
            await _mediator.Publish(e).ConfigureAwait(continueOnCapturedContext: false);
            await _eventSourcingRepository.SaveEventAsync(e).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task PublishNotificationAsync<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task PublishDomainEventAsync<T>(T notification) where T : DomainEvent
        {
            await _mediator.Publish(notification).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}