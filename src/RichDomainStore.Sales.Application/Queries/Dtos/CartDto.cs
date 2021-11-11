using System;
using System.Collections.Generic;

namespace RichDomainStore.Sales.Application.Queries.Dtos
{
    public class CartDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalValue { get; set; }
        public decimal DiscountValue { get; set; }
        public string VoucherCode { get; set; }

        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public CartPaymentDto Payment { get; set; }
    }
}