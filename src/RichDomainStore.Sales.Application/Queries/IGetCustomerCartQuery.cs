using System;
using System.Threading.Tasks;
using RichDomainStore.Sales.Application.Queries.Dtos;

namespace RichDomainStore.Sales.Application.Queries
{
    public interface IGetCustomerCartQuery
    {
        Task<CartDto> HandleAsync(Guid customerId);
    }
}