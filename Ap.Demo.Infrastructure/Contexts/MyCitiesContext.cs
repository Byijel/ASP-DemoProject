using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Ap.Demo.Infrastructure.Seeding;

namespace Ap.Demo.Infrastructure.Contexts
{
    public class MyCitiesContext : DbContext
    {
        public MyCitiesContext(DbContextOptions<MyCitiesContext> options)
            : base(options) { }

        public DbSet<City> Cities => Set<City>();
        public DbSet<Country> Countries => Set<Country>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyCitiesContext).Assembly);

            // Seeding
            modelBuilder.SeedCountries();
            modelBuilder.SeedCities();
        }
    }
}
