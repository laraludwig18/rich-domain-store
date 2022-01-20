using System;

namespace RichDomainStore.Catalog.Application.Dtos
{
    public class AddProductDto
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal Value { get; set; }

        public string Image { get; set; }

        public int StockQuantity { get; set; }

        public decimal Height { get; set; }

        public decimal Width { get; set; }

        public decimal Depth { get; set; }
    }
}