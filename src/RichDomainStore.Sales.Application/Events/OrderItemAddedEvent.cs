using System;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Sales.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal Value { get; private set; }
        public int Quantity { get; private set; }

        public OrderItemAddedEvent(Guid customerId, Guid orderId, Guid productId, string productName, decimal value, int quantity)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Value = value;
            Quantity = quantity;
        }
    }
}