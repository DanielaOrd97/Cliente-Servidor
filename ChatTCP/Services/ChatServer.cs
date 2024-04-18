using ChatTCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatTCP.Services
{
    public class ChatServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient>();


        public void Iniciar()
        {
            //empieza a escuchar cualquier conexion a traves del puerto 9000.
            server = new(new IPEndPoint(IPAddress.Any, 9000));
            server.Start();

           new Thread(Escuchar)
           {
               IsBackground = true   //evita el bloqueo de interfaz.
           }.Start();  
        }

        public void Detener()
        {
            if(server != null)
            {
                server.Stop();

                foreach (var c in clients)
                {
                    c.Close(); //Desconecta a todos los usuarios.
                }
                clients.Clear();
            }
        }

        void Escuchar()
        {
            while (server.Server.Connected)
            {
                var tcpClient = server.AcceptTcpClient();
                clients.Add(tcpClient);

                Thread t = new(() =>
                {
                    RecibirMensajes(tcpClient);
                });

                t.IsBackground = true;
                t.Start();
            }
        }

        public EventHandler<MensajeDTO>? MensajeRecibido; //Lanza a viewmodel.
        void RecibirMensajes(TcpClient client)
        {

            while (client.Connected)
            {
                var ns = client.GetStream();

                while(client.Available == 0)
                {
                    Thread.Sleep(500);
                }

                byte[] buffer = new byte[client.Available];
                ns.Read(buffer, 0, buffer.Length);
                var json = Encoding.UTF8.GetString(buffer);
                var mensaje = JsonSerializer.Deserialize<MensajeDTO>(json);

                if(mensaje != null)
                {
                    //Reenviar el mensaje a los otros clientes y a la interfaz grafica con un evento.
                    ReenviarMensaje(client, buffer);
                    MensajeRecibido?.Invoke(this, mensaje);
                }
            }

            clients.Remove(client);
        }

        void ReenviarMensaje(TcpClient cliente, byte[] mensaje)
        {
            foreach (TcpClient c in clients)
            {
                if(c != cliente && c.Connected) //Enviar a todos menos el origen y verificar que siga conectado el cliente.
                {
                    var ns = c.GetStream();
                    ns.Write(mensaje, 0, mensaje.Length);
                    ns.Flush(); ///sin cerrar el stream.
                }
            }
        }
    }
}
