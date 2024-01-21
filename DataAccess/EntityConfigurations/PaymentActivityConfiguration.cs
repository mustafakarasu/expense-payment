using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class PaymentActivityConfiguration : IEntityTypeConfiguration<PaymentActivity>
{
    public void Configure(EntityTypeBuilder<PaymentActivity> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ExpenseId).IsRequired();
        builder.Property(x => x.PaidDate).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.CurrencyType).IsRequired(false).HasMaxLength(5);
    }
}