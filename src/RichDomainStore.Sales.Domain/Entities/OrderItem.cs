using System;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Sales.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }

        // EF Rel.
        public Order Order { get; set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal value)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Value = value;
        }

        protected OrderItem() { }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculateValue()
        {
            return Quantity * Value;
        }

        internal void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }

        internal void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}