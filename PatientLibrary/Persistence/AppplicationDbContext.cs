using Microsoft.EntityFrameworkCore;
using PatientLibrary.Models;
using PatientLibrary.Persistence.Configurations;

namespace PatientLibrary.Persistence
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Name> Names { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new NameConfiguration());
        }
    }
}
