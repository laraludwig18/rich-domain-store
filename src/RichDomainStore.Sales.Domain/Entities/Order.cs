using System;
using System.Collections.Generic;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Enums;

namespace RichDomainStore.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool HasVoucherBeenUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        // EF Rel.
        public Voucher Voucher { get; private set; }

        public Order(Guid customerId, bool hasVoucherBeenUsed, decimal discount, decimal totalValue)
        {
            CustomerId = customerId;
            HasVoucherBeenUsed = hasVoucherBeenUsed;
            Discount = discount;
            TotalValue = totalValue;
            _orderItems = new List<OrderItem>();
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }
    }
}