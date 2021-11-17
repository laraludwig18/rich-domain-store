using System;
using FluentValidation;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.Sales.Application.Validators
{
    public class ApplyVoucherValidator : AbstractValidator<ApplyVoucherCommand>
    {
        public ApplyVoucherValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId is invalid");

            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage("VoucherCode cannot be empty");
        }
    }
}