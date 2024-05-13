using System;
using System.Collections.Generic;

namespace NoticiasProyecto.Models.Entities;

public partial class Usuarios
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool? EsAdmin { get; set; }

    public virtual ICollection<Noticias> Noticias { get; set; } = new List<Noticias>();
}
