using Ap.Demo.Application.CQRS.City;
using Ap.Demo.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ap.Demo.API.Controllers
{
    public class CityController : APIv1Controller
    {
        private readonly IMediator mediator;

        public CityController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            return Ok(await mediator.Send(new GetAllCitiesQuery() { SortOrder = "usc"}));
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityDto city)
        {
            if (id != city.Id) return BadRequest();
            return Ok(await mediator.Send(new UpdateCitiesCommand() { City = city}));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            return Ok(await mediator.Send(new GetCityByIdQuery { Id = id }));
        }
    }
}
