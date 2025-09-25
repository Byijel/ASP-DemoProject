using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Application.CQRS.City
{
    public class UpdateCityDTO
    {
        public int Id { get; set; }
        public long Population { get; set; }
    }
}
