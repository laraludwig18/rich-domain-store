using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class CancelOrderProcessReversingStockCommand : Command
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public CancelOrderProcessReversingStockCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CancelOrderProcessReversingStockValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}