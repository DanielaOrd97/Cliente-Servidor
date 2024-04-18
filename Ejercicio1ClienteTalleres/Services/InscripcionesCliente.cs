using Ejercicio1ClienteTalleres.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ejercicio1ClienteTalleres.Services
{
    public class InscripcionesCliente
    {
        //clase que se encarga de enviar inscripciones a un servidor a través de UDP.


        UdpClient cliente = new();

        //propiedad que especifica la dirección IP del servidor al que se enviarán las inscripciones.
        public string Servidor { get; set; } = "0.0.0.0";

        public void EnviarInscripcion(InscripcionDTO dto)
        {
            var ipe = new IPEndPoint(IPAddress.Parse(Servidor), 5001);
            var json = JsonSerializer.Serialize(dto);  //objeto a cadena de texto estructurada.
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            cliente.Send(buffer, buffer.Length, ipe);
        }
    }
}
