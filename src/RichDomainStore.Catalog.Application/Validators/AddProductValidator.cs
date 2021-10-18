using FluentValidation;
using RichDomainStore.Catalog.Application.DTOS;

namespace RichDomainStore.Catalog.API.FluentValidation
{
    public class AddProductValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty();
            RuleFor(p => p.Description).NotNull().NotEmpty();
            RuleFor(p => p.CategoryId).NotNull().NotEmpty();
            RuleFor(p => p.Value).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Image).NotEmpty();
            RuleFor(p => p.Height).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Width).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Depth).GreaterThanOrEqualTo(1);
        }
    }
}