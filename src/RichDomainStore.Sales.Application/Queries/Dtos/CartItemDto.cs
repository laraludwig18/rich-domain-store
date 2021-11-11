using System;

namespace RichDomainStore.Sales.Application.Queries.Dtos
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public decimal TotalValue { get; set; }
    }
}