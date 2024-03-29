using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Validators
{
    public class CancelOrderProcessReversingStockValidator : AbstractValidator<CancelOrderProcessReversingStockCommand>
    {
        public CancelOrderProcessReversingStockValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("OrderId is invalid");
                
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");
        }
    }
}