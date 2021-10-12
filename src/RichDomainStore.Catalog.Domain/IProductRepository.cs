using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Core.Data;

namespace RichDomainStore.Catalog.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategoryCode(int code);
        Task<IEnumerable<Category>> GetCategories();

        void Add(Product product);
        void Update(Product product);

        void Add(Category category);
        void Update(Category category);
    }
}