using RichDomainStore.Core.Data;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Domain.Interfaces
{
    public class IOrderRepository : IRepository<Order>
    {
        public IUnitOfWork UnitOfWork => throw new System.NotImplementedException();

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}