using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaJWT1.Helpers;

namespace PruebaJWT1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            if(password == "ITESRC")
            {
                JwtTokerGenerator jwtToken = new();
                return Ok(jwtToken.GetToken(username));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
