using Ap.Demo.Application.CQRS.Country;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ap.Demo.API.Controllers
{
    public class CountriesController : APIv1Controller
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(result);
        }
    }
}