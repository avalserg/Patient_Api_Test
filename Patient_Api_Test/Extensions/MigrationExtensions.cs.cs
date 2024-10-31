using Microsoft.EntityFrameworkCore;
using PatientLibrary.Persistence;

namespace Patient_Api_Test.Extensions
{
    internal static class MigrationExtensions
    {
        internal static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            ApplyMigration<ApplicationDbContext>(scope);

        }

        private static void ApplyMigration<TDbContext>(IServiceScope scope)
            where TDbContext : DbContext
        {
            using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            context.Database.Migrate();
        }
    }
}
