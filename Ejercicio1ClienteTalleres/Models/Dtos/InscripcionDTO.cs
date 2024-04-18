using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1ClienteTalleres.Models.Dtos
{
    public class InscripcionDTO
    {

        //clase que representa un objeto de transferencia de datos (DTO) para almacenar información sobre una inscripción.
        public string Nombre { get; set; } = "";
        public string Taller { get; set; } = "";
    }
}
