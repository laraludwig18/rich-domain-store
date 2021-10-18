using System;

namespace RichDomainStore.Catalog.Application.DTOS
{
    public class UpdateProductDTO
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal Value { get; set; }

        public string Image { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Depth { get; set; }
    }
}