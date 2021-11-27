using RichDomainStore.Payments.Business.Dtos;
using RichDomainStore.Payments.Business.Entities;

namespace RichDomainStore.Payments.Business.Interfaces
{
    public interface ICreditCardPaymentFacade
    {
        Transaction PerformPayment(OrderDto order, Payment payment);
    }
}