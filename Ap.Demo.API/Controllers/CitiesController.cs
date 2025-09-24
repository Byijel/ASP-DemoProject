using Ap.Demo.Application.CQRS.City;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ap.Demo.API.Controllers
{
    public class CitiesController : APIv1Controller
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetAll([FromQuery] string sortOrder = "asc")
        {
            var result = await _mediator.Send(new GetAllCitiesQuery { SortOrder = sortOrder });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CityDTO>> Add([FromBody] CityDTO dto)
        {
            var result = await _mediator.Send(new AddCommand { City = dto });
            return CreatedAtAction(nameof(GetAll), new { sortOrder = "asc" }, result);
        }
    }
}
