using System;
using System.Threading.Tasks;

namespace RichDomainStore.Catalog.Domain
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(Guid productId, int quantity);
        Task<bool> ReStock(Guid productId, int quantity);
    }
}