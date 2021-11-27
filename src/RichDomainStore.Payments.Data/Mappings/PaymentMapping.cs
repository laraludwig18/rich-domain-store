using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichDomainStore.Payments.Business.Entities;

namespace RichDomainStore.Payments.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CardName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.CardNumber)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(c => c.CardExpiration)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.CardSecurityCode)
                .IsRequired()
                .HasColumnType("varchar(4)");

            // 1 : 1 => Payment : Transaction
            builder.HasOne(c => c.Transaction)
                .WithOne(c => c.Payment);

            builder.ToTable("Payments");
        }
    }
}