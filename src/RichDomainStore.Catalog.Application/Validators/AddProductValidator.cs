using System;
using FluentValidation;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.ValueObjects;

namespace RichDomainStore.API.FluentValidation
{
    public class AddProductValidator : AbstractValidator<AddProductDto>
    {
        public AddProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Name is empty");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Description is empty");

            RuleFor(p => p.CategoryId)
                .NotEqual(Guid.Empty)
                .WithMessage("CategoryId is invalid");

            RuleFor(p => p.Value)
                .GreaterThanOrEqualTo(Product.MinValue)
                .WithMessage($"Value must be greather than {Product.MinValue}");

            RuleFor(p => p.Image)
                .NotEmpty()
                .WithMessage("Image is empty");

            RuleFor(p => p.Height)
                .GreaterThanOrEqualTo(Dimensions.MinHeight)
                .WithMessage($"Height must be greather than {Dimensions.MinHeight}");

            RuleFor(p => p.Width)
                .GreaterThanOrEqualTo(Dimensions.MinWidth)
                .WithMessage($"Width must be greather than {Dimensions.MinWidth}");

            RuleFor(p => p.Depth)
                .GreaterThanOrEqualTo(Dimensions.MinDepth)
                .WithMessage($"Depth must be greather than {Dimensions.MinDepth}");
        }
    }
}