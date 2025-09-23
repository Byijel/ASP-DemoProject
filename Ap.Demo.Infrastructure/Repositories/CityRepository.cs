using Ap.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Contexts;

namespace Ap.Demo.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MyCitiesContext _context;

        public CityRepository(MyCitiesContext context)
        {
            _context = context;
        }

        public Task<City> Add(City entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(City entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<City>> GetAll(string sortOrder = "asc")
        {
            var query = _context.Cities.Include(c => c.Country).AsQueryable();
            return await (sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(c => c.Population)
                : query.OrderBy(c => c.Population)).ToListAsync();
        }

        public Task<IEnumerable<City>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<City?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public City Update(City entinty)
        {
            throw new NotImplementedException();
        }
    }
}