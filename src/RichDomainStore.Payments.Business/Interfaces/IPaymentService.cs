using System.Threading.Tasks;
using RichDomainStore.Core.DomainObjects.Dtos;
using RichDomainStore.Payments.Business.Entities;

namespace RichDomainStore.Payments.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<Transaction> PerformOrderPayment(OrderPaymentDto orderPayment);
    }
}