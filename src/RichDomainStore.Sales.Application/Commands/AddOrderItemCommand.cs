using System;
using RichDomainStore.Core.Messages;
using RichDomainStore.Sales.Application.Validators;

namespace RichDomainStore.Sales.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }

        public AddOrderItemCommand(Guid customerId, Guid productId, string productName, int quantity, decimal value)
        {
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Value = value;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}