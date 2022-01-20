using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Entities;

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
                .WithMessage("ProductName is required");

            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(OrderItem.MinItemQuantity)
                .WithMessage($"The minimum quantity of an item is {OrderItem.MinItemQuantity}")
                .LessThanOrEqualTo(OrderItem.MaxItemQuantity)
                .WithMessage($"The maximum quantity of an item is {OrderItem.MaxItemQuantity}");

            RuleFor(c => c.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greather than 0");
        }
    }
}