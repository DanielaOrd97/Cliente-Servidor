using ITESRCLibrosAPI.Models.Dtos;
using ITESRCLibrosAPI.Models.Entities;
using ITESRCLibrosAPI.Models.Validators;
using ITESRCLibrosAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITESRCLibrosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        public LibrosController(LibrosRepository repository)
        {
            Repository = repository;
        }

        public LibrosRepository Repository { get; }

        [HttpPost]
        public IActionResult Post(LibroDTO dto)
        {
            //1.- Validar.

            //if (string.IsNullOrWhiteSpace(dto.Titulo)) //VALIDACION MANUAL************
            //{
            //    return BadRequest("El titulo esta vacio.");
            //}


            //VALIDACION CON FLUENT*********************
            //Install-Package FluentValidation
            LibroValidator validator = new();
            var resultados = validator.Validate(dto);

            //2.- Si es valido, agregar.
            if (resultados.IsValid)
            {
                //crear entidad
                Libros entity = new()
                {
                    Id = 0,
                    Eliminado = false,
                    FechaActualizacion = DateTime.Now,
                    Autor = dto.Autor,    //MAPEO
                    Portada = dto.Portada, //MAPEO
                    Titulo = dto.Titulo //MAPEO
                };
                Repository.Insert(entity);
                return Ok();
            }
            
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));


            
        }


        //[HttpGet("{fecha?:datetime}")]
        [HttpGet("{fecha?}")]
        public IActionResult Get(DateTime? fecha)
        {
            var libros = Repository.GetAll()
                .Where(x => fecha == null || x.FechaActualizacion > fecha)
                .OrderBy(x => x.Titulo)
                .Select(x => new LibroDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Autor = x.Autor,
                    Eliminado = x.Eliminado,
                    Portada = x.Portada
                });

            return Ok(libros);
        }
    }
}
