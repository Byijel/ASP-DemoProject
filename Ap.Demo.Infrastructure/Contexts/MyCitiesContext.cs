using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Ap.Demo.Infrastructure.Seeding;
using System.Reflection;
using Ap.Demo.Infrastructure.Configuration;

namespace Ap.Demo.Infrastructure.Contexts
{
    public class MyCitiesContext : DbContext
    {
        public MyCitiesContext(DbContextOptions<MyCitiesContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyCitiesContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());


            modelBuilder.Entity<City>().Seed();
            modelBuilder.Entity<Country>().Seed();
        }
    }
}
