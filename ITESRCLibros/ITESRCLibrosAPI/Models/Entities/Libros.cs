using System;
using System.Collections.Generic;

namespace ITESRCLibrosAPI.Models.Entities;

public partial class Libros
{
    public int? Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public string Portada { get; set; } = null!;

    public DateTime FechaActualizacion { get; set; }

    public bool Eliminado { get; set; }
}
