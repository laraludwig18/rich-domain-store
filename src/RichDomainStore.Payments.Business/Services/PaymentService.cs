using System.Threading.Tasks;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.DomainObjects.Dtos;
using RichDomainStore.Core.Messages.CommonMessages.IntegrationEvents;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Payments.Business.Dtos;
using RichDomainStore.Payments.Business.Entities;
using RichDomainStore.Payments.Business.Enums;
using RichDomainStore.Payments.Business.Interfaces;

namespace RichDomainStore.Payments.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardPaymentFacade _creditCardPaymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PaymentService(ICreditCardPaymentFacade creditCardPaymentFacade,
                                IPaymentRepository paymentRepository,
                                IMediatorHandler mediatorHandler)
        {
            _creditCardPaymentFacade = creditCardPaymentFacade;
            _paymentRepository = paymentRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transaction> PerformOrderPayment(OrderPaymentDto orderPayment)
        {
            var order = new OrderDto
            {
                Id = orderPayment.OrderId,
                Value = orderPayment.Total
            };

            var payment = new Payment
            {
                Value = orderPayment.Total,
                CardName = orderPayment.CardName,
                CardNumber = orderPayment.CardNumber,
                CardExpiration = orderPayment.CardExpiration,
                CardSecurityCode = orderPayment.CardSecurityCode,
                OrderId = orderPayment.OrderId
            };

            var transaction = _creditCardPaymentFacade.PerformPayment(order, payment);

            if (transaction.TransactionStatus == TransactionStatus.Paid)
            {
                payment.AddEvent(
                        new OrderPaymentPerformedEvent(order.Id,
                            orderPayment.CustomerId,
                            transaction.PaymentId,
                            transaction.Id,
                            order.Value));

                _paymentRepository.Add(payment);
                _paymentRepository.AddTransaction(transaction);

                await _paymentRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
                return transaction;
            }

            await _mediatorHandler.PublishNotificationAsync(
                    new DomainNotification("payment", "The operator refused payment")).ConfigureAwait(continueOnCapturedContext: false);

            await _mediatorHandler.PublishEventAsync(
                    new OrderPaymentDeniedEvent(order.Id, 
                        orderPayment.CustomerId, 
                        transaction.PaymentId, 
                        transaction.Id, 
                        order.Value)).ConfigureAwait(continueOnCapturedContext: false);

            return transaction;
        }
    }
}