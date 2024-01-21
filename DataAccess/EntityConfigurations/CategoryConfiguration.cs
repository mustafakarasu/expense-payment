using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(25);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(450);
        }
    }
}
