using System;

namespace RichDomainStore.Catalog.Application.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }
    }
}