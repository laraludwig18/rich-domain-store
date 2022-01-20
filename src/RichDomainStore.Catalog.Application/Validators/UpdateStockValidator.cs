using FluentValidation;
using RichDomainStore.Catalog.Application.Dtos;

namespace RichDomainStore.Catalog.Application.Validators
{
    public class UpdateStockValidator : AbstractValidator<UpdateStockDto>
    {
        public UpdateStockValidator()
        {
            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Quantity must be greather than 1");
        }
    }
}