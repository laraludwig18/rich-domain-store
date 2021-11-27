using RichDomainStore.Core.Data;
using RichDomainStore.Payments.Business.Entities;

namespace RichDomainStore.Payments.Business.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Add(Payment payment);

        void AddTransaction(Transaction transaction);
    }
}