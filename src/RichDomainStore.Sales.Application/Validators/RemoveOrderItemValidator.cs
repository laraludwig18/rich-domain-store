using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Validators
{
    public class RemoveOrderItemValidator : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("ProductId is invalid");
        }
    }
}