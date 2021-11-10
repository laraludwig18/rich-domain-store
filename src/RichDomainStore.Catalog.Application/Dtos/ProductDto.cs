using System;

namespace RichDomainStore.Catalog.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal Value { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Image { get; set; }

        public int StockQuantity { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Depth { get; set; }

        public CategoryDto Category { get; set; }
    }
}