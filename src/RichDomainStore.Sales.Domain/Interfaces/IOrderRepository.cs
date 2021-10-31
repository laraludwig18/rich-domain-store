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
        void AddAsync(Order order);
        void UpdateAsync(Order order);

        Task<OrderItem> GetItemByIdAsync(Guid id);
        Task<OrderItem> GetItemByOrderIdAsync(Guid orderId, Guid productId);
        void AddItemAsync(OrderItem orderItem);
        void UpdateItemAsync(OrderItem orderItem);
        void RemoveItemAsync(OrderItem orderItem);

        Task<Voucher> GetVoucherByCodeAsync(string code);
    }
}