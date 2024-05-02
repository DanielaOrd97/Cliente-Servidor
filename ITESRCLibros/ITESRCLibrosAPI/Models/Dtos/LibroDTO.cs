
namespace ITESRCLibrosAPI.Models.Dtos
{
    public class LibroDTO
    {
        public int? Id { get; set; } //Interesa el id, guarda en la base de datos.

        public string Titulo { get; set; } = null!;
        public DateTime Fecha { get; set; }

        public string Autor { get; set; } = null!;

        public string Portada { get; set; } = null!;

        public bool Eliminado { get; set; } //para eliminar en la base de datos.
       
    }
}

