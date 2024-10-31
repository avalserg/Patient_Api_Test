using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientLibrary.Models;

namespace PatientLibrary.Persistence.Configurations
{
    public sealed class NameConfiguration : IEntityTypeConfiguration<Name>
    {
        public void Configure(EntityTypeBuilder<Name> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Family).IsRequired();
        }
    }
}
