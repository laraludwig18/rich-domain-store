using System;

namespace RichDomainStore.Catalog.Application.DTOS
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }
    }
}