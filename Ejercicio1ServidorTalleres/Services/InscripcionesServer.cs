using Ejercicio1ServidorTalleres.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Ejercicio1ServidorTalleres.Services
{
    public class InscripcionesServer
    {
        

        public InscripcionesServer()
        {
            //delegado que apunta al metodo Iniciar.
          var hilo =  new Thread(new ThreadStart(Iniciar));
            hilo.IsBackground = true;
            hilo.Start();
        }

        //Delegado
        public event EventHandler<InscripcionDto>? InscripcionRealizada;

        //programa independiente
        void Iniciar()
        {
            UdpClient server = new UdpClient(5001);

            while(true)
            {
                IPEndPoint remoto = new(IPAddress.Any, 5001);
                byte[] buffer = server.Receive(ref remoto);

                InscripcionDto? dto = JsonSerializer.Deserialize<InscripcionDto>(Encoding.UTF8.GetString(buffer));

                if (dto != null)
                {
                    //Invoca el evento en el hilo principal.
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        InscripcionRealizada?.Invoke(this, dto);
                    });
                    //Invoke(origen, contenido que envia)
                    
                }
            }

           
        }
    }
}
