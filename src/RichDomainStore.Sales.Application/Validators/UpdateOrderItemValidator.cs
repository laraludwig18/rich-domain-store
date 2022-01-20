using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Application.Validators
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("ProductId is invalid");

            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(OrderItem.MinItemQuantity)
                .WithMessage($"The minimum quantity of an item is {OrderItem.MinItemQuantity}")
                .LessThanOrEqualTo(OrderItem.MaxItemQuantity)
                .WithMessage($"The maximum quantity of an item is {OrderItem.MaxItemQuantity}");
        }
    }
}