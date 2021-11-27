using RichDomainStore.Core.Data;
using RichDomainStore.Payments.Business.Entities;
using RichDomainStore.Payments.Business.Interfaces;

namespace RichDomainStore.Payments.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}