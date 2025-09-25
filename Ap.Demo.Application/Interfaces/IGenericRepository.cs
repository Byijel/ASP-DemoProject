
namespace Ap.Demo.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T> Add(T entity);
        T Update(T city);
        void Delete(T entity);
    }
}
