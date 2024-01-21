using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.UserName).IsRequired(false);
        builder.HasIndex(x => x.NormalizedUserName).IsUnique(false).HasDatabaseName("UserNameIndex");

        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.NormalizedEmail).IsUnique().HasDatabaseName("EmailIndex");

        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(25);
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(25);

        var defaultAdmin = new DefaultAdminSettings();
        var passwordHasher = new PasswordHasher<User>();
        defaultAdmin.Admin1.PasswordHash = passwordHasher.HashPassword(defaultAdmin.Admin1, DefaultAdminSettings.PasswordAdmin1);
        defaultAdmin.Admin2.PasswordHash = passwordHasher.HashPassword(defaultAdmin.Admin2, DefaultAdminSettings.PasswordAdmin2);

        builder.HasData(defaultAdmin.Admin1);
        builder.HasData(defaultAdmin.Admin2);
    }
}