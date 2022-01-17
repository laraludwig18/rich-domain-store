using System.Collections.Generic;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public int Code { get; private set; }

        // EF Relation
        public ICollection<Product> Products { get; set; }
        protected Category() { }

        public Category(string name, int code)
        {
            Name = name;
            Code = code;

            Validate();
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }

        private void Validate()
        {
            AssertionConcern.AssertArgumentNotEmpty(Name, "Category name cannot be empty");
            AssertionConcern.AssertArgumentEquals(Code, 0, "Category code cannot be equal to 0");
        }
    }
}