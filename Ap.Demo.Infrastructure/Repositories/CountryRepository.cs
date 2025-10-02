using Ap.Demo.Application.Interfaces;
using Ap.Demo.Domain;
using Ap.Demo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly MyCitiesContext context;
        public CountryRepository(MyCitiesContext context) : base(context)
        {
            this.context = context;
        }

        public new async Task<Country?> GetById(int id)
        {
            return await context.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Country>> GetAll()
        {
            return await context.Countries
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }


    }
}
