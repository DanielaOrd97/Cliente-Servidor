using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoticiasProyecto.Helpers;
using NoticiasProyecto.Models.DTOs;
using NoticiasProyecto.Models.Entities;
using NoticiasProyecto.Repositories;
using System.Security.Claims;

namespace NoticiasProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {

        public IRepository<Usuarios> Repo { get; }
        public JwtHelper Jwthelp { get; }

        public LogInController(IRepository<Usuarios> repo, JwtHelper jwthelp)
        {
            Repo = repo;
            Jwthelp = jwthelp;
        }


        [HttpPost]
        public IActionResult Authenticate(LogInDto dto)
        {
            var usuario = Repo.GetAll().FirstOrDefault(x => x.NombreUsuario == dto.Usuario && x.Contraseña == dto.Password);

            if (usuario == null)
            {
                return Unauthorized();
            }

            var token = Jwthelp.GetToken(usuario.Nombre, usuario.EsAdmin == true ? "Admin" : "Periodista", new List<Claim> { new Claim("Id", usuario.Id.ToString())});

            return Ok(token);
        }
    }
}
