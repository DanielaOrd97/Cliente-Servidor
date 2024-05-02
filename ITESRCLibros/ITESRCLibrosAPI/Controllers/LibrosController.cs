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
                    FechaActualizacion = DateTime.UtcNow,
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
        [HttpGet("{fecha?}/{hora?}/{minutos?}")]
        public IActionResult Get(DateTime? fecha, int hora = 0, int minutos = 0)
        {
            if (fecha != null)
            {
                fecha = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day,
                   hora, minutos, 0);
            }

                var libros = Repository.GetAll()
                .Where(x => fecha == null || x.FechaActualizacion > fecha)
                .OrderBy(x => x.Titulo)
                .Select(x => new LibroDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Autor = x.Autor,
                    Eliminado = x.Eliminado,
                    Portada = x.Portada,
                    Fecha =x.FechaActualizacion
                });

            return Ok(libros);
        }


        [HttpPut("{id}")]
        public IActionResult Put(LibroDTO dto)
        {
            LibroValidator validator = new();
            var resultados = validator.Validate(dto);

            //2.- Si es valido, agregar.
            if (resultados.IsValid)
            {
                //Buscar la entidad del dto
                var entidadLibro = Repository.Get(dto.Id ?? 0);

                //verificar si se encontro (encontrado, no encontrado, encontrado pero esta borrado (baja logica))
                if(entidadLibro == null || entidadLibro.Eliminado == true)
                {
                    return NotFound();
                }
                else
                {
                    entidadLibro.Autor = dto.Autor;
                    entidadLibro.Titulo = dto.Titulo;
                    entidadLibro.Portada = dto.Portada;
                    entidadLibro.FechaActualizacion = DateTime.UtcNow;

                    Repository.Update(entidadLibro);

                    return Ok();

                }

                
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));



        }


        [HttpDelete ("{id}")] //No acepta Dtos, solo ids
        public IActionResult Delete(int id)
        {
            var entidadLibro = Repository.Get(id);

            if(entidadLibro == null || entidadLibro.Eliminado)
            {
                return NotFound();
            }

            entidadLibro.Eliminado = true;
            entidadLibro.FechaActualizacion = DateTime.UtcNow;
            Repository.Update(entidadLibro);

            return Ok();
        }
    }
}
