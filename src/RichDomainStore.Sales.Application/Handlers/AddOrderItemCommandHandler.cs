using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Bus;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AddOrderItemCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            return true;
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid())
            {
                return true;
            }

            foreach (var error in message.ValidationResult.Errors)
            {
                // _mediatorHandler.PublishEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}