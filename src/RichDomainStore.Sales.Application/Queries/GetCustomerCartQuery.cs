using System;
using System.Threading.Tasks;
using RichDomainStore.Sales.Application.Queries.Dtos;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.Sales.Application.Queries
{
    public class GetCustomerCartQuery : IGetCustomerCartQuery
    {
        private readonly IOrderRepository _orderRepository;
        
        public GetCustomerCartQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CartDto> HandleAsync(Guid customerId)
        {
            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId).ConfigureAwait(false);
            if (order == null) 
            {
                return null;
            }

            var cart = new CartDto
            {
                CustomerId = order.CustomerId,
                TotalValue = order.TotalValue,
                OrderId = order.Id,
                DiscountValue = order.Discount,
                SubTotal = order.Discount + order.TotalValue
            };

            if (order.VoucherId != null)
            {
                cart.VoucherCode = order.Voucher.Code;
            }

            foreach (var item in order.OrderItems)
            {
                cart.Items.Add(new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Value = item.Value,
                    TotalValue = item.Value * item.Quantity
                });
            }

            return cart;
        }
    }
}