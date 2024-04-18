using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1ServidorTalleres.Models
{
    public class Taller
    {
        public string NombreTaller { get; set; } = null!;
        public List<Alumno> ListaAlumnos { get; set; } = new List<Alumno>();
    }
}
