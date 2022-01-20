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
        public decimal? DiscountPercentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Quantity { get; private set; }
        public VoucherDiscountType DiscountType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        // EF Rel.
        public ICollection<Order> Orders { get; set; }

        public Voucher(
            string code,
            int quantity,
            VoucherDiscountType discountType,
            DateTime expirationDate,
            bool active,
            bool used,
            decimal discountPercentage = default(decimal),
            decimal discountValue = default(decimal))
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            DiscountValue = discountValue;
            Quantity = quantity;
            DiscountType = discountType;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
            CreatedAt = DateTime.Now;
        }

        protected Voucher() { }

        internal ValidationResult ValidateIfApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    internal class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public VoucherApplicableValidation()
        {
            RuleFor(c => c.Code)
               .NotEmpty()
               .WithMessage("Voucher code cannot be empty");

            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateGreaterThanActual)
                .WithMessage("Voucher is expired");

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage("Voucher is not valid");

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage("Voucher has already been used");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("Voucher not available");

            When(f => f.DiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(f => f.DiscountValue)
                    .GreaterThan(0)
                    .WithMessage("Voucher discount value must be greather than 0");
            });

            When(f => f.DiscountType == VoucherDiscountType.Percentage, () =>
            {
                RuleFor(f => f.DiscountPercentage)
                    .GreaterThan(0)
                    .WithMessage("Voucher discount percentage must be greather than 0");
            });
        }

        protected static bool ExpirationDateGreaterThanActual(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}