using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Location).IsRequired().HasMaxLength(45);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(450);
        builder.Property(x => x.CurrencyType).IsRequired(false).HasMaxLength(5);
    }
}