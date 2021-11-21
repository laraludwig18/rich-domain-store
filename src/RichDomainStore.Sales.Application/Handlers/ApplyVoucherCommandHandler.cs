using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Events;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Handlers
{
    public class ApplyVoucherCommandHandler : CommandHandler, IRequestHandler<ApplyVoucherCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplyVoucherCommandHandler(
            IOrderRepository orderRepository,
            IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(ApplyVoucherCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(message.CustomerId).ConfigureAwait(false);
            if (order == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Order not found")).ConfigureAwait(false);
                return false;
            }

            var voucher = await _orderRepository.GetVoucherByCodeAsync(message.VoucherCode).ConfigureAwait(false);
            if (voucher == null)
            {
                await _mediatorHandler.PublishNotificationAsync(new DomainNotification("order", "Voucher not found"));
                return false;
            }

            var validationResult = order.ApplyVoucher(voucher);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    await _mediatorHandler.PublishNotificationAsync(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            order.AddEvent(new VoucherAppliedEvent(message.CustomerId, order.Id, voucher.Id));

            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}