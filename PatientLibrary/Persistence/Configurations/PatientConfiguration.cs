using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientLibrary.Enums;
using PatientLibrary.Models;

namespace PatientLibrary.Persistence.Configurations
{
    public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {

            builder.Property(p => p.BirthDate).IsRequired();

            builder
                .Property(e => e.Gender)
                .HasConversion(
                    v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v));

            builder.HasOne(p => p.Name)
                .WithOne()
                .HasForeignKey<Name>(n => n.Id);
        }
    }
}
