using System;

namespace RichDomainStore.Sales.Application.Queries.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderStatus { get; set; }
    }
}