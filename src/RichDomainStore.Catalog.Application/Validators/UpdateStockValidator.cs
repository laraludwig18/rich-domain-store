using FluentValidation;
using RichDomainStore.Catalog.Application.DTOS;

namespace RichDomainStore.Catalog.Application.Validators
{
    public class UpdateStockValidator : AbstractValidator<UpdateStockDTO>
    {
        public UpdateStockValidator()
        {
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(1);
        }
    }
}