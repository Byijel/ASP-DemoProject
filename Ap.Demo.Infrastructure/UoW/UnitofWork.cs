
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Contexts;
using Ap.Demo.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.UoW
{
    public class UnitofWork : IUnitofWork
    {
        private readonly MyCitiesContext _context;

        public ICityRepository Cities { get; private set; }

        public UnitofWork(MyCitiesContext context)
        {
            _context = context;
            Cities = new CityRepository(_context);
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
