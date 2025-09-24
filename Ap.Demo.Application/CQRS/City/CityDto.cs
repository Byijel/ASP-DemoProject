using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Application.CQRS.City
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Population { get; set; }
        public int CountryId { get; set; }
    }
}
