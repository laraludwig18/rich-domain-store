using System;
using System.Collections.Generic;

namespace RichDomainStore.Payments.Business.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}