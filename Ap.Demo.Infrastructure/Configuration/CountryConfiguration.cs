using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ap.Demo.Infrastructure.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("tblCountries", "Country")
                   .HasKey(co => co.Id);

            builder.Property(co => co.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnType("int");

            builder.Property(co => co.Name)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnType("nvarchar(50)");
        }
    }
}