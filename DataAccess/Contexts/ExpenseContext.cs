using System.Globalization;
using DataAccess.EntityConfigurations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class ExpenseContext : IdentityDbContext<User, Role, int>
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentActivity> PaymentActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
            AdjustIdentityTableNames(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await DateTimeChangesForBaseEntityProperties();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private Task DateTimeChangesForBaseEntityProperties()
        {
            var dateUtcNow = DateTime.UtcNow;
            ChangeTracker.DetectChanges();

            foreach ( var entityEntry in ChangeTracker.Entries().Where(x => x.Entity.GetType().BaseType == typeof(BaseEntity)) )
            {
                var baseEntity = (BaseEntity)entityEntry.Entity;
                switch ( entityEntry.State )
                {
                    case EntityState.Added:
                        baseEntity.CreatedDate = dateUtcNow;
                        break;
                    case EntityState.Modified:
                        baseEntity.ModifiedDate = dateUtcNow;
                        break;
                }
            }
            return Task.CompletedTask;
        }

        private void AdjustIdentityTableNames(ModelBuilder builder)
        {
            var identityTables = builder.Model.GetEntityTypes()
                .Where(x => x.GetTableName()!.ToLower().Contains("aspnet"));

            foreach ( var mutableEntityType in identityTables )
            {
                mutableEntityType.SetTableName(mutableEntityType.GetTableName()!.Replace("aspnet", "", true, CultureInfo.InvariantCulture));
            }
        }
    }
}
