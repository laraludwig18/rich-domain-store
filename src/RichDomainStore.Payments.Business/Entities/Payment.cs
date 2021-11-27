using System;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Payments.Business.Entities
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public decimal Value { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public string CardSecurityCode { get; set; }

        // EF. Rel.
        public Transaction Transaction { get; set; }
    }
}