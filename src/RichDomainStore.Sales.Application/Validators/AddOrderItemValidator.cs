using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Validators
{
    public class AddOrderItemValidator : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("ProductId is invalid");

            RuleFor(c => c.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("The minimum quantity of an item is 1");

            RuleFor(c => c.Quantity)
                .LessThan(15)
                .WithMessage("The maximum quantity of an item is 15");

            RuleFor(c => c.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greather than 1");
        }
    }
}