using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ap.Demo.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class APIv1Controller : ControllerBase
    {
    }
}
