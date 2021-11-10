using AutoMapper;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Domain;
using RichDomainStore.Catalog.Domain.Entities;

namespace RichDomainStore.Catalog.Application.Mappers
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<AddProductDto, Product>()
                .ConstructUsing(p =>
                    new Product(p.Name,
                        p.Description,
                        p.Active,
                        p.Value,
                        p.CategoryId,
                        p.Image,
                        new Dimensions(
                            p.Height,
                            p.Width,
                            p.Depth)));

            CreateMap<UpdateProductDto, Product>()
                .ConstructUsing(p =>
                    new Product(p.Name,
                        p.Description,
                        p.Active,
                        p.Value,
                        p.CategoryId,
                        p.Image,
                        new Dimensions(
                            p.Height,
                            p.Width,
                            p.Depth)));
        }
    }
}