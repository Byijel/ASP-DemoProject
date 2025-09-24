using Ap.Demo.Domain;

namespace Ap.Demo.Application.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<IEnumerable<City>> GetAll(string sortOrder = "asc");

        Task<string> GetByName(string name);
    }
}