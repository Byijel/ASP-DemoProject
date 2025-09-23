using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.Seeding
{
    public static class CountrySeeding
    {
        public static void SeedCountries(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "België" },
                new Country { Id = 2, Name = "Nederland" },
                new Country { Id = 3, Name = "Frankrijk" }
            );
        }
    }
}
