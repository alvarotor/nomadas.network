using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Nomadas.Network.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok("OK");
        }
    }
}
