using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class RemoveOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }

        public RemoveOrderItemCommand(Guid customerId, Guid productId)
        {
            CustomerId = customerId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}