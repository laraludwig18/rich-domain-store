using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichDomainStore.Sales.Domain.Entities;

namespace RichDomainStore.Sales.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Code)
                .IsRequired()
                .HasColumnType("varchar(100)");

            // 1 : N => Voucher : Orders
            builder.HasMany(v => v.Orders)
                .WithOne(v => v.Voucher)
                .HasForeignKey(v => v.VoucherId);

            builder.ToTable("Vouchers");
        }
    }
}