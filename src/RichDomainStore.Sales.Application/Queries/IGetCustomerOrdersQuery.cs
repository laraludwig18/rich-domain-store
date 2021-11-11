using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Sales.Application.Queries.Dtos;

namespace RichDomainStore.Sales.Application.Queries
{
    public interface IGetCustomerOrdersQuery
    {
        Task<IEnumerable<OrderDto>> HandleAsync(Guid customerId);
    }
}