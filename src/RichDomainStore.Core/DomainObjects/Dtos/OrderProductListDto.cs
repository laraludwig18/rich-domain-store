using System;
using System.Collections.Generic;

namespace RichDomainStore.Core.DomainObjects.Dtos
{
    public class OrderProductListDto
    {
        public Guid OrderId { get; set; }
        public IEnumerable<ItemDto> Items { get; set; }
    }
}