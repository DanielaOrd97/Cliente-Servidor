using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
//using SQLite.Net.Attributes;
//using TableAttribute = SQLite.Net.Attributes.TableAttribute;

namespace LibrosITESRCMAUI.Models.Entities
{
    //MODIFICAR *************
    // [SQLite.Net.Attributes.Table("Libros")]
    [SQLite.Table("Libros")]
    public class Libro
    {
        [PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; } = null!;

        [NotNull]
        public string Autor { get; set; } = null!;

        [NotNull]
        public string Portada { get; set; } = null!;
        public bool Eliminado { get; set; }
    }
}
