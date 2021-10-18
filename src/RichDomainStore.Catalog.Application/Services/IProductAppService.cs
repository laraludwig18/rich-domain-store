using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Application.DTOS;

namespace RichDomainStore.Catalog.Application.Services
{
    public interface IProductAppService : IDisposable
    {
        Task<IEnumerable<ProductDTO>> GetByCategoryCodeAsync(int code);
        Task<ProductDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();

        Task<ProductDTO> AddProductAsync(AddProductDTO product);
        Task<ProductDTO> UpdateProductAsync(UpdateProductDTO product);

        Task<ProductDTO> DebitStockAsync(Guid id, UpdateStockDTO updateStockDTO);
        Task<ProductDTO> ReStockAsync(Guid id, UpdateStockDTO updateStockDTO);
    }
}