using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Application.Dtos;

namespace RichDomainStore.Catalog.Application.Services
{
    public interface IProductAppService : IDisposable
    {
        Task<IEnumerable<ProductDTO>> GetByCategoryCode(int code);
        Task<ProductDTO> GetById(Guid id);
        Task<IEnumerable<ProductDTO>> GetAll();
        Task<IEnumerable<CategoryDTO>> GetCategories();

        Task AddProduct(ProductDTO product);
        Task UpdateProduct(ProductDTO product);

        Task<ProductDTO> DebitStock(Guid id, int quantity);
        Task<ProductDTO> ReStock(Guid id, int quantity);
    }
}