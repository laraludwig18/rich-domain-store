using RichDomainStore.Payments.AntiCorruption.Interfaces;
using RichDomainStore.Payments.Business.Dtos;
using RichDomainStore.Payments.Business.Entities;
using RichDomainStore.Payments.Business.Enums;
using RichDomainStore.Payments.Business.Interfaces;

namespace RichDomainStore.Payments.AntiCorruption.Implementations
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Transaction PerformPayment(OrderDto order, Payment payment)
        {
            var cardHashKey = GenerateCardHashKey(payment.CardNumber);

            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, order.Id.ToString(), payment.Value);

            var transaction = new Transaction
            {
                OrderId = order.Id,
                Total = order.Value,
                PaymentId = payment.Id
            };

            if (paymentResult)
            {
                transaction.TransactionStatus = TransactionStatus.Paid;
                return transaction;
            }

            transaction.TransactionStatus = TransactionStatus.Denied;
            return transaction;
        }

        private string GenerateCardHashKey(string cardNumber)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encryptionKey = _configManager.GetValue("encryptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encryptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, cardNumber);

            return cardHashKey;
        }
    }
}