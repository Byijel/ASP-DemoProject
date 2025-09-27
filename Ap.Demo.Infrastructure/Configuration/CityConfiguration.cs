using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ap.Demo.Infrastructure.Configuration
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("tblCities", "City")
                   .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnType("int");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnType("nvarchar(100)");

            builder.Property(c => c.Population)
                   .IsRequired()
                   .HasColumnType("long");

            builder.Property(c => c.CountryId)
                   .IsRequired()
                   .HasColumnType("int");

            builder.HasOne(c => c.Country)
                   .WithMany(co => co.Cities)
                   .HasForeignKey(c => c.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}