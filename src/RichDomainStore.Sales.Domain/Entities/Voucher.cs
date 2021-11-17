using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Enums;

namespace RichDomainStore.Sales.Domain.Entities
{
    public class Voucher : Entity
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Quantity { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        // EF Rel.
        public ICollection<Order> Orders { get; set; }

        internal ValidationResult ValidateIfApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {

        public VoucherApplicableValidation()
        {
            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateGreaterThanActual)
                .WithMessage("Voucher is expired.");

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage("Voucher is not valid.");

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage("Voucher has already been used.");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("Voucher not available.");
        }

        protected static bool ExpirationDateGreaterThanActual(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}