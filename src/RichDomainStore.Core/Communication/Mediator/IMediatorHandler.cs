using System.Threading.Tasks;
using RichDomainStore.Core.Messages;
using RichDomainStore.Core.Messages.CommonMessages.DomainEvents;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;

namespace RichDomainStore.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEventAsync<T>(T e) where T : Event;
        Task<bool> SendCommandAsync<T>(T command) where T : Command;
        Task PublishNotificationAsync<T>(T notification) where T : DomainNotification;
        Task PublishDomainEventAsync<T>(T notification) where T : DomainEvent;
    }
}