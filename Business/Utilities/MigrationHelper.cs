using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Utilities
{
    public static class MigrationHelper
    {
        public static void MigrateDatabase(this IServiceProvider provider)
        {
            using ( var scope = provider.CreateScope() )
            {
                using ( var appContext = scope.ServiceProvider.GetRequiredService<ExpenseContext>() )
                {
                    appContext.Database.Migrate();
                }
            }
        }
    }
}
