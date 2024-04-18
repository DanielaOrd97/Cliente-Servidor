using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajeServer.Models
{
    public class Mensaje
    {
        public string Texto { get; set; } = "";
        public string ColorFondo { get; set; } = "#fff";
        public string ColorLetra { get; set; } = "#000";
    }
}
