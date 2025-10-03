using System.ComponentModel.DataAnnotations;

namespace Ap.Demo.Application.CQRS.City
{
    public class UpdateCityDTO
    {
        public int Id { get; set; }

        [Range(1, 1000000000, ErrorMessage = "Population must be between 1 and 1,000,000,000")]
        public long Population { get; set; }
    }
}
