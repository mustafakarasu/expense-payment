using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(x => x.IsApproved).IsRequired();
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(450);

        builder
            .HasOne(x => x.Expense)
            .WithOne(x => x.Payment)
            .HasForeignKey<Payment>(x => x.ExpenseId);
    }
}