using System;
using System.Collections.Generic;
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
    }
}