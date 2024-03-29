using System;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Sales.Domain.Entities
{
    public class OrderItem : Entity
    {
        public const int MinItemQuantity = 1;
        public const int MaxItemQuantity = 15;
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

            Validate();
        }

        protected OrderItem() { }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        internal decimal CalculateValue()
        {
            return Quantity * Value;
        }

        internal void IncrementQuantity(int quantity)
        {
            Quantity += quantity;

            ValidateQuantity();
        }

        internal void UpdateQuantity(int quantity)
        {
            Quantity = quantity;

            ValidateQuantity();
        }

        private void Validate()
        {
            ValidateQuantity();
        }

        private void ValidateQuantity()
        {
            AssertionConcern.AssertArgumentRange(
                Quantity,
                MinItemQuantity,
                MaxItemQuantity,
                $"Product quantity cannot be less than {MinItemQuantity} or greather than {MaxItemQuantity}");
        }
    }
}