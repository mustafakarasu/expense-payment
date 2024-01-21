using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasData(new IdentityUserRole<int>()
        {
            RoleId = RoleSettings.AdminRoleId,
            UserId = RoleSettings.IdAdmin1
        },
            new IdentityUserRole<int>()
            {
                RoleId = RoleSettings.AdminRoleId,
                UserId = RoleSettings.IdAdmin2
            });
    }
}