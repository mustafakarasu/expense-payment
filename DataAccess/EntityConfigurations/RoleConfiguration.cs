using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = RoleSettings.AdminRoleId,
                Name = RoleSettings.Admin,
                NormalizedName = RoleSettings.NormalizedAdmin
            },
            new Role
            {
                Id = RoleSettings.EmployeeRoleId,
                Name = RoleSettings.Employee,
                NormalizedName = RoleSettings.NormalizedEmployee
            });
    }
}