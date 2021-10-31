using System;

namespace RichDomainStore.API.Models
{
    public class AddOrderItemRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}