using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class StartOrderCommand : Command
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Total { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpiration { get; private set; }
        public string CardSecurityCode { get; private set; }

        public StartOrderCommand(Guid orderId,
            Guid customerId,
            decimal total,
            string cardName,
            string cardNumber,
            string cardExpiration,
            string cardSecurityCode)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardSecurityCode = cardSecurityCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new StartOrderValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}