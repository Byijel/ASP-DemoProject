using Ap.Demo.Application.CQRS.City;
using FluentValidation;
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
            try
            {
                var result = await _mediator.Send(new AddCommand { City = dto });
                return CreatedAtAction(nameof(GetAll), new { sortOrder = "asc" }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                // Log the full exception details
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                var fullMessage = $"Error: {ex.Message}. Inner: {innerMessage}";

                // In development, return the full error
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    InnerException = innerMessage,
                    StackTrace = ex.StackTrace // Remove this in production
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDTO city)
        {
            if (id != city.Id) return BadRequest();
            return Ok(await _mediator.Send(new UpdateCitiesCommand() { City = city}));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            return Ok(await _mediator.Send(new GetCityByIdQuery { Id = id }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
				await _mediator.Send(new DeleteCityQuery(id));
				return NoContent();
			}
            catch (InvalidOperationException ex)
            {
				//last city case
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				//city not found
				return NotFound(ex.Message);
			}
		}
    }
}
