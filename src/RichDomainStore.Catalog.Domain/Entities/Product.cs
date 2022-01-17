using System;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public const decimal MinValue = 1;
        public const int LowStockQuantity = 10;
        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Value { get; private set; }
        public DateTime CreatedAt { get; private set; }
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
            CreatedAt = DateTime.Now;
            Image = image;
            Dimensions = dimensions;

            Validate();
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
            Description = description;

            ValidateDescription();
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

        public bool IsLowStock()
        {
            return StockQuantity < LowStockQuantity;
        }

        private void Validate()
        {
            AssertionConcern.AssertArgumentNotEmpty(Name, "Product name cannot be empty");
            AssertionConcern.AssertArgumentNotEquals(CategoryId, Guid.Empty, "Product category id cannot be empty");
            AssertionConcern.AssertArgumentGreaterThan(Value, MinValue, $"Product value cannot be less than {MinValue}");
            AssertionConcern.AssertArgumentNotEmpty(Image, "Product image cannot be empty");

            ValidateDescription();
        }

        private void ValidateDescription()
        {
            AssertionConcern.AssertArgumentNotEmpty(Description, "Product description cannot be empty");
        }
    }
}