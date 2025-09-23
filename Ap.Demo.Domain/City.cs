using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Domain
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Population { get; set; }

        // Foreign key naar Country
        public int CountryId { get; set; }
        public Country? Country { get; set; }
    }
}
