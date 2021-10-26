using System;
using System.Collections.Generic;
using System.Linq;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Sales.Domain.Enums;

namespace RichDomainStore.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool HasVoucherBeenUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        // EF Rel.
        public Voucher Voucher { get; private set; }

        public Order(Guid customerId, bool hasVoucherBeenUsed, decimal discount, decimal totalValue)
        {
            CustomerId = customerId;
            HasVoucherBeenUsed = hasVoucherBeenUsed;
            Discount = discount;
            TotalValue = totalValue;
            _orderItems = new List<OrderItem>();
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void ApplyVoucher(Voucher voucher)
        {
            Voucher = voucher;
            HasVoucherBeenUsed = true;
            CalculateOrderValue();
        }

        public void CalculateOrderValue()
        {
            TotalValue = OrderItems.Sum(i => i.CalculateValue());
            CalculateTotalDiscountValue();
        }

        public void CalculateTotalDiscountValue()
        {
            if (!HasVoucherBeenUsed)
            {
                return;
            };

            var discount = Voucher.VoucherDiscountType switch
            {
                VoucherDiscountType.Percentage => CalculatePercentageDiscount(),
                VoucherDiscountType.Value => Voucher.DiscountValue.HasValue ? Voucher.DiscountValue.Value : 0,
                _ => 0
            };

            var totalValue = TotalValue -= discount;

            TotalValue = totalValue < 0 ? 0 : totalValue;
            Discount = discount;
        }

        private decimal CalculatePercentageDiscount()
        {
            if (!Voucher.Percentage.HasValue)
            {
                return 0;
            }

            return TotalValue * Voucher.Percentage.Value / 100;
        }

        public bool CheckOrderItemExists(OrderItem item)
        {
            return _orderItems.Any(p => p.ProductId == item.ProductId);
        }

        public void AddItem(OrderItem item)
        {
            if (!item.IsValid())
            {
                return;
            }

            item.AssociateOrder(Id);

            if (CheckOrderItemExists(item))
            {
                var existentItem = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);
                existentItem.IncreaseQuantity(item.Quantity);
                item = existentItem;

                _orderItems.Remove(existentItem);
            }

            _orderItems.Add(item);

            CalculateOrderValue();
        }

        public void RemoveItem(OrderItem item)
        {
            if (!item.IsValid())
            {
                return;
            }

            var existentItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (existentItem == null)
            {
                throw new DomainException("O item não pertence ao pedido");
            }

            _orderItems.Remove(existentItem);

            CalculateOrderValue();
        }

        public void UpdateItem(OrderItem item)
        {
            if (!item.IsValid())
            {
                return;
            }

            item.AssociateOrder(Id);

            var existentItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (existentItem == null)
            {
                throw new DomainException("Item does not exists in the order");
            }

            _orderItems.Remove(existentItem);
            _orderItems.Add(item);

            CalculateOrderValue();
        }

        public void UpdateItemQuantity(OrderItem item, int units)
        {
            item.UpdateQuantity(units);
            UpdateItem(item);
        }

        public void MakeOrderADraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public void StartOrder()
        {
            OrderStatus = OrderStatus.Started;
        }

        public void FinishOrder()
        {
            OrderStatus = OrderStatus.Paid;
        }

        public void CancelOrder()
        {
            OrderStatus = OrderStatus.Canceled;
        }

        public static class OrderFactory
        {
            public static Order NewDraft(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.MakeOrderADraft();
                return order;
            }
        }
    }
}