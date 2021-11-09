using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;

namespace RichDomainStore.Sales.Application.Handlers
{
    public abstract class CommandHandler
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CommandHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        protected bool ValidateCommand(Command message)
        {
            if (message.IsValid())
            {
                return true;
            }

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotificationAsync(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}