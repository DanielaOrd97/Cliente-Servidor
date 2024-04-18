using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatosController : ControllerBase
    {
        [HttpGet]   
        public IActionResult Get()
        {
            // return Ok("Hola mundo");
            string[] datos = new string[] { "Hola mundo", "Adios mundo" };
            return Ok(datos);
        }

        [HttpPost]
        public IActionResult Post(int numero)
        {
            return Ok(numero * numero);
            //return Created("numero", numero * numero);
        }

        [HttpGet("numero")]
        public IActionResult GetNumero()
        {
            return Ok(1000);
        }

        [HttpGet("{nombre}")]
        public IActionResult Saludar(string nombre)
        {
            return Ok("Hola "+nombre);
        }
    }
}
