using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RichDomainStore.Core.Data;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<Order> GetDraftOrderByCustomerIdAsync(Guid customerId);
        void Add(Order order);
        void Update(Order order);

        Task<OrderItem> GetItemByIdAsync(Guid id);
        Task<OrderItem> GetItemByOrderIdAsync(Guid orderId, Guid productId);
        void AddItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
        void RemoveItem(OrderItem orderItem);

        Task<Voucher> GetVoucherByCodeAsync(string code);
    }
}