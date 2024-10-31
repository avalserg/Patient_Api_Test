using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatientLibrary.Persistence;

namespace PatientLibrary
{
    public static class PatientLibraryInjection
    {
        public static IServiceCollection PatientLibraryExtension(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<ApplicationDbContext>((sp, options) =>
                options.UseNpgsql(configuration.GetConnectionString("Database"), npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, "patients")));
        }
    }
}
