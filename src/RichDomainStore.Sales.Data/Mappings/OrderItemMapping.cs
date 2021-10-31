using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Data.Mappings
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            // 1 : N => Order : OrderItems
            builder.HasOne(oi => oi.Order)
                .WithMany(oi => oi.OrderItems);

            builder.ToTable("OrderItems");
        }
    }
}