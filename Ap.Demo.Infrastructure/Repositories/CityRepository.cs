using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Contexts;

namespace Ap.Demo.Infrastructure.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly MyCitiesContext _context;

        public CityRepository(MyCitiesContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> GetAll(string sortOrder = "asc")
        {
            var query = _context.Cities.Include(c => c.Country).AsQueryable();
            return await (sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(c => c.Population)
                : query.OrderBy(c => c.Population)).ToListAsync();
        }
        public async Task<City> Add(City entity)
        {
            await _context.Cities.AddAsync(entity);
            return entity;
        }

        public async Task<City?> GetByNameAndCountryId(string name, int countryId)
        {
            return await _context.Cities
                .FirstOrDefaultAsync(c => c.Name == name && c.CountryId == countryId);
        }
    }
}