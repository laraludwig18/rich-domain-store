using System;
using System.Threading.Tasks;
using RichDomainStore.Core.DomainObjects.Dtos;

namespace RichDomainStore.Catalog.Domain.Interfaces
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockAsync(Guid productId, int quantity);
        Task<bool> DebitOrderProductListAsync(OrderProductListDto list);
        Task<bool> ReStockAsync(Guid productId, int quantity);
        Task<bool> ReStockOrderProductListAsync(OrderProductListDto list);
    }
}