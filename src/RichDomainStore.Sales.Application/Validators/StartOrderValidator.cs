using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Validators
{
    public class StartOrderValidator : AbstractValidator<StartOrderCommand>
    {
        public StartOrderValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("OrderId is invalid");

            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");

            RuleFor(c => c.CardName)
                .NotEmpty()
                .WithMessage("CardName is empty");

            RuleFor(c => c.CardNumber)
                .CreditCard()
                .WithMessage("CardNumber is invalid");

            RuleFor(c => c.CardExpiration)
                .NotEmpty()
                .WithMessage("CardExpiration is empty");

            RuleFor(c => c.CardSecurityCode)
                .Length(3, 4)
                .WithMessage("CardSecurityCode is invalid");
        }
    }
}