using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(35);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(450);
    }
}