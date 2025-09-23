using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ap.Demo.Application.CQRS.City;

namespace Ap.Demo.WebUi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAll([FromQuery] string sortOrder = "asc")
        {
            var query = new GetAllCitiesQuery { SortOrder = sortOrder };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}