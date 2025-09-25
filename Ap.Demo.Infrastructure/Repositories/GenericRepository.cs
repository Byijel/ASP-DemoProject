using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyCitiesContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(MyCitiesContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetById(int id)
        {
            var city = await dbSet.FindAsync(id);
            return city;
        }

        public T Update(T city)
        {
            dbSet.Update(city);

            return city;
        }
    }
}
