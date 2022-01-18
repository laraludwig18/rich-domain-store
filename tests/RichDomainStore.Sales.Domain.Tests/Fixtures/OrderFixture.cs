using System;
using Bogus;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Enums;
using Xunit;

namespace RichDomainStore.Sales.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(OrderCollection))]
    public class OrderCollection : ICollectionFixture<OrderFixture>
    { }

    public class OrderFixture : IDisposable
    {
        public Order GenerateDraftOrder()
        {
            return Order.OrderFactory.NewDraft(customerId: Guid.NewGuid());
        }

        public OrderItem GenerateValidOrderItem(Guid productId = default(Guid), int itemQuantity = 1, decimal value = 1)
        {
            return new Faker<OrderItem>("pt_BR")
                .CustomInstantiator(faker => new OrderItem(
                    productId: productId == Guid.Empty ? Guid.NewGuid() : productId,
                    productName: faker.Commerce.ProductName(),
                    quantity: itemQuantity,
                    value: value));
        }

        public Voucher GenerateValidVoucher(
            VoucherDiscountType discountType,
            decimal discountPercentage = default(decimal),
            decimal discountValue = default(decimal))
        {
            return new Faker<Voucher>("pt_BR")
                .CustomInstantiator(faker => new Voucher(
                    code: "PROMO-10-OFF",
                    discountPercentage: discountPercentage,
                    discountValue: discountValue,
                    quantity: faker.Random.Int(min: 1),
                    discountType: discountType,
                    expirationDate: DateTime.Now.AddDays(10),
                    active: true,
                    used: false));
        }

        public Voucher GenerateInvalidVoucher(
            VoucherDiscountType discountType,
            decimal discountPercentage = default(decimal),
            decimal discountValue = default(decimal))
        {
            return new Faker<Voucher>("pt_BR")
                .CustomInstantiator(faker => new Voucher(
                    code: string.Empty,
                    discountPercentage: discountPercentage,
                    discountValue: discountValue,
                    quantity: 0,
                    discountType: discountType,
                    expirationDate: DateTime.Now.AddDays(-1),
                    active: false,
                    used: true));
        }

        public void Dispose()
        {
        }
    }
}