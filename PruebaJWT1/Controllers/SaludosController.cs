using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaJWT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaludosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hola a todos");
        }
    }
}
