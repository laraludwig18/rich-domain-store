using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Application.Dtos;

namespace RichDomainStore.Catalog.Application.Services
{
    public interface IProductAppService : IDisposable
    {
        Task<IEnumerable<ProductDto>> GetByCategoryCodeAsync(int code);
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        Task<ProductDto> AddProductAsync(AddProductDto product);
        Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto product);

        Task<ProductDto> DebitStockAsync(Guid id, UpdateStockDto updateStockDTO);
        Task<ProductDto> ReStockAsync(Guid id, UpdateStockDto updateStockDTO);
    }
}