using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.FolderPath).IsRequired().HasMaxLength(250);
    }
}