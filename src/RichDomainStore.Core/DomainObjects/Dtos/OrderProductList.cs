using System;
using System.Collections.Generic;

namespace RichDomainStore.Core.DomainObjects.Dtos
{
    public class OrderProductList
    {
        public Guid OrderId { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}