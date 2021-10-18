using System;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Value { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Image { get; private set; }
        public int StockQuantity { get; private set; }
        public Dimensions Dimensions { get; private set; }
        public Category Category { get; private set; }

        protected Product() { }
        
        public Product(
            string name,
            string description,
            bool active,
            decimal value,
            Guid categoryId,
            string image,
            Dimensions dimensions)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Active = active;
            Value = value;
            RegisterDate = DateTime.Now;
            Image = image;
            Dimensions = dimensions;

            this.Validate();
        }

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        public void ChangeCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public void ChangeDescription(string description)
        {
            AssertionConcern.AssertArgumentNotEmpty(Description, "Product description cannot be empty");
            Description = description;
        }

        public void DebitStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            if (!HasStock(quantity)) throw new DomainException("Insufficient stock");
            StockQuantity -= quantity;
        }

        public void ReStock(int quantity)
        {
            StockQuantity += quantity;
        }

        public bool HasStock(int quantity)
        {
            return StockQuantity >= quantity;
        }

        private void Validate()
        {
            AssertionConcern.AssertArgumentNotEmpty(Name, "Product name cannot be empty");
            AssertionConcern.AssertArgumentNotEmpty(Description, "Product description cannot be empty");
            AssertionConcern.AssertArgumentNotEquals(CategoryId, Guid.Empty, "Product category id cannot be empty");
            AssertionConcern.AssertArgumentGreaterThan(Value, 1, "Product value cannot be less than 1");
            AssertionConcern.AssertArgumentNotEmpty(Image, "Product image cannot be empty");
        }
    }
}