using System;
using System.Threading.Tasks;

namespace RichDomainStore.Catalog.Domain.Interfaces
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockAsync(Guid productId, int quantity);
        Task<bool> ReStockAsync(Guid productId, int quantity);
    }
}