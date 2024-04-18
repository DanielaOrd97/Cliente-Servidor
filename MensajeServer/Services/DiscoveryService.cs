using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MensajeServer.Services
{
    public class DiscoveryService
    {
        UdpClient server = new() { 
        EnableBroadcast = true,
        };

        IPEndPoint destino = new IPEndPoint(IPAddress.Broadcast, 7000);

        byte[] buffer;

        public DiscoveryService()
        {
            var usuario = Environment.UserName;     
            buffer = Encoding.UTF8.GetBytes(usuario);

            Saludar(); //cuando se crea, este saluda a la red.
            new Thread(RecibirSaludo) { IsBackground = true }.Start();//esperar que nos saluden.

            new Thread(StillAlive) { IsBackground = true }.Start(); //Informar que sigue vivo.
        }

        public void Saludar()
        {
            server.Send(buffer, buffer.Length, destino);    
        }

        private void StillAlive()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                Saludar();
            }
        }

        private void RecibirSaludo() //responder el saludo cuando me saludan
        {
            UdpClient udp2 = new(7001);

            while (true)
            {
                IPEndPoint remoto = new(IPAddress.Any, 0); //guarda quien lo mando
                byte[] buffer = udp2.Receive(ref remoto);

                server.Send(buffer, buffer.Length, remoto);
            }
        }
    }
}
