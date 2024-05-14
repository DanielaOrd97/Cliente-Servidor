using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoticiasProyecto.Models.Entities;
using NoticiasProyecto.Repositories;

namespace NoticiasProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodistasController : ControllerBase
    {

        private IRepository<Usuarios> UsuariosRepository;

        public PeriodistasController(IRepository<Usuarios> usuariosrepository)
        {
                UsuariosRepository = usuariosrepository;
        }


        [HttpGet]
        public IActionResult Get()
        {

        }
    }
}
