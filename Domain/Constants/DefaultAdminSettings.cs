using Domain.Entities;

namespace Domain.Constants
{
    public class DefaultAdminSettings
    {
        const string EmailAdmin1 = "mustafa.karasu@admin.com";
        const string EmailAdmin2 = "karasu.mustafa@admin.com";
        public const string PasswordAdmin1 = "Patika2024Mustafa";
        public const string PasswordAdmin2 = "Akbank2024Mustafa";

        public User Admin1 { get; } = new()
        {
            Id = RoleSettings.IdAdmin1,
            FirstName = "Mustafa",
            LastName = "Karasu",
            Email = EmailAdmin1,
            NormalizedEmail =EmailAdmin1.ToUpperInvariant().Normalize(),
            UserName = EmailAdmin1,
            NormalizedUserName = EmailAdmin1.ToUpperInvariant().Normalize(),
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        public User Admin2 { get; } = new()
        {
            Id = RoleSettings.IdAdmin2,
            FirstName = "Karasu",
            LastName = "Mustafa",
            Email = EmailAdmin2,
            NormalizedEmail =EmailAdmin2.ToUpperInvariant().Normalize(),
            UserName = EmailAdmin2,
            NormalizedUserName = EmailAdmin2.ToUpperInvariant().Normalize(),
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
    }
}
