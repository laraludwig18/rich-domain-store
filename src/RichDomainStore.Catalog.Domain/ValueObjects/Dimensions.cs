using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Domain
{
    public class Dimensions
    {
        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }

        public Dimensions(decimal height, decimal width, decimal depth)
        {
            AssertionConcern.AssertArgumentGreaterThan(height, 1, "Height cannot be less than 1");
            AssertionConcern.AssertArgumentGreaterThan(width, 1, "Width cannot be less than 1");
            AssertionConcern.AssertArgumentGreaterThan(depth, 1, "Depth cannot be less than 1");

            Height = height;
            Width = width;
            Depth = depth;
        }

        public string FormattedDescription()
        {
            return $"WxHxD: {Width} x {Height} x {Depth}";
        }

        public override string ToString()
        {
            return FormattedDescription();
        }
    }
}