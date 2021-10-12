using AutoMapper;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Domain;

namespace RichDomainStore.Catalog.Application.Mappers
{
    public class DTOToDomainMappingProfile : Profile
    {
        public DTOToDomainMappingProfile()
        {
            CreateMap<ProductDTO, Product>()
                .ConstructUsing(p =>
                    new Product(p.Name,
                        p.Description,
                        p.Active,
                        p.Value,
                        p.CategoryId,
                        p.RegisterDate,
                        p.Image,
                        new Dimensions(
                            p.Height,
                            p.Width,
                            p.Depth)));

            CreateMap<CategoryDTO, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));
        }
    }
}