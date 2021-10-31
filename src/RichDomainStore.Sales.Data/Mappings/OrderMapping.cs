using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence");

            // 1 : N => Order : OrderItems
            builder.HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId);

            builder.ToTable("Orders");
        }
    }
}