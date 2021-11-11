using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RichDomainStore.Sales.Application.Queries.Dtos;
using RichDomainStore.Sales.Domain.Enums;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Queries
{
    public class GetCustomerOrdersQuery : IGetCustomerOrdersQuery
    {
        private readonly IOrderRepository _orderRepository;
        
        public GetCustomerOrdersQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> HandleAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId).ConfigureAwait(false);

            orders = orders.Where(p => p.OrderStatus == OrderStatus.Paid || p.OrderStatus == OrderStatus.Canceled)
                .OrderByDescending(p => p.Code);

            if (!orders.Any()) 
            {
                return null;
            }

            var ordersDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                ordersDto.Add(new OrderDto
                {
                    Id = order.Id,
                    TotalValue = order.TotalValue,
                    OrderStatus = (int)order.OrderStatus,
                    Code = order.Code,
                    CreatedAt = order.CreatedAt
                });
            }

            return ordersDto;
        }
    }
}