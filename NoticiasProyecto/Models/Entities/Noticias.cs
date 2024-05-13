using System;
using System.Collections.Generic;

namespace NoticiasProyecto.Models.Entities;

public partial class Noticias
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public string? ImagenUrl { get; set; }

    public DateOnly FechaPublicacion { get; set; }

    public int? AutorId { get; set; }

    public virtual Usuarios? Autor { get; set; }
}
