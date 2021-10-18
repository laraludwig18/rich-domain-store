using AutoMapper;
using RichDomainStore.Catalog.Application.DTOS;
using RichDomainStore.Catalog.Domain;
using RichDomainStore.Catalog.Domain.Entities;

namespace RichDomainStore.Catalog.Application.Mappers
{
    public class DTOToDomainMappingProfile : Profile
    {
        public DTOToDomainMappingProfile()
        {
            CreateMap<AddProductDTO, Product>()
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

            CreateMap<UpdateProductDTO, Product>()
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