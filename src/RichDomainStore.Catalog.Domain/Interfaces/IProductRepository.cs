using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Core.Data;

namespace RichDomainStore.Catalog.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetByCategoryCodeAsync(int code);
        Task<IEnumerable<Category>> GetCategoriesAsync();

        void Add(Product product);
        void Update(Product product);

        void Add(Category category);
        void Update(Category category);
    }
}