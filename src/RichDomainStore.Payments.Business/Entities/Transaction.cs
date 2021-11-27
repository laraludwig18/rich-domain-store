using System;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Payments.Business.Enums;

namespace RichDomainStore.Payments.Business.Entities
{
    public class Transaction : Entity
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Total { get; set; }
        public TransactionStatus TransactionStatus { get; set; }

        // EF. Rel.
        public Payment Payment { get; set; }
    }
}