using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.Seeding
{
    public static class CitySeeding
    {
        public static void SeedCities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Antwerpen", Population = 530000, CountryId = 1 },
                new City { Id = 2, Name = "Gent", Population = 260000, CountryId = 1 },
                new City { Id = 3, Name = "Amsterdam", Population = 820000, CountryId = 2 },
                new City { Id = 4, Name = "Parijs", Population = 2140000, CountryId = 3 }
            );
        }
    }
}
