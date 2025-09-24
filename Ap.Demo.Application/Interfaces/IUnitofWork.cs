using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Application.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        ICityRepository CityRepository { get; }

        ICountryRepository CountryRepository { get; }
        Task Commit();
    }
}
