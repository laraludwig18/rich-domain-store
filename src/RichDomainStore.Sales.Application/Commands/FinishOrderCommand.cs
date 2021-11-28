using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class FinishOrderCommand : Command
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public FinishOrderCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public override bool IsValid()
        {
            ValidationResult = new FinishOrderValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}