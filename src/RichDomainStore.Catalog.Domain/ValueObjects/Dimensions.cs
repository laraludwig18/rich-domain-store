using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Domain
{
    public class Dimensions
    {
        public const decimal MinHeight = 1;
        public const decimal MinWidth = 1;
        public const decimal MinDepth = 1;
        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }

        public Dimensions(decimal height, decimal width, decimal depth)
        {
            AssertionConcern.AssertArgumentGreaterThan(height, MinHeight, $"Height cannot be less than {MinHeight}");
            AssertionConcern.AssertArgumentGreaterThan(width, MinWidth, $"Width cannot be less than {MinWidth}");
            AssertionConcern.AssertArgumentGreaterThan(depth, MinDepth, $"Depth cannot be less than {MinDepth}");

            Height = height;
            Width = width;
            Depth = depth;
        }

        private string FormatDescripton()
        {
            return $"WxHxD: {Width} x {Height} x {Depth}";
        }

        public override string ToString()
        {
            return FormatDescripton();
        }
    }
}