using AutoMapper;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Domain.Entities;

namespace RichDomainStore.Catalog.Application.Mappers
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Width, o => o.MapFrom(s => s.Dimensions.Width))
                .ForMember(d => d.Height, o => o.MapFrom(s => s.Dimensions.Height))
                .ForMember(d => d.Depth, o => o.MapFrom(s => s.Dimensions.Depth));

            CreateMap<Category, CategoryDto>();
        }
    }
}