using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1ServidorTalleres.Models.Dtos
{
    //POCO: Plain Old C# Object (solo propiedades)
    public class InscripcionDto
    {
        public string Nombre { get; set; } = "";
        public string Taller { get; set; } = "";
    }
}
