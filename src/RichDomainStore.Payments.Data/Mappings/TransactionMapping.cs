using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichDomainStore.Payments.Business.Entities;

namespace RichDomainStore.Payments.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
         public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Transactions");
        }
    }
}