using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaJWT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SaludosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var context = HttpContext;
            if (User.Identity != null)
            {
                return Ok("Hola " + User.Identity.Name);
            }
                
            return Ok("Hola a todos");
        }
    }
}
