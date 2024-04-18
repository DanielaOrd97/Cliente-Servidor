using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TCPChatServer.Models;

namespace TCPChatServer.Services
{
    public class ChatServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient>();

        public void Iniciar()
        {
            //empieza a escuchar cualquier conexion a traves del puerto 9000.
            server = new(new IPEndPoint(IPAddress.Any, 9000));

            //se inicia el server.
            server.Start();

            //crea hilos para escuchar a cada cliente y asi evita el bloqueo de interfaz.
            new Thread(Escuchar)
            {
                //es hilo secundario.
                //si el proceso principal finaliza, los hilos secundarios en segundo plano también se detendrán, incluso si están en medio de alguna operación.
                IsBackground = true
            }.Start();
        }
        
        public void Detener()
        {
            if(server != null)
            {
                server.Stop();

                foreach (var c in clients)
                {
                    //Desconecta a todos los usuarios.
                    c.Close(); 
                }
                clients.Clear();
            }
        }

        private void Escuchar()
        {
            //Verifica que el servidor este escuchando.
            while (server.Server.IsBound)
            {
                //Acepta al cliente.
                var tcpClient = server.AcceptTcpClient();

                //Agrega al cliente a la lista de clientes.
                clients.Add(tcpClient);

                //Creo hilos para recibir mensajes de cada cliente.
                Thread t = new(() =>
                {
                    RecibirMensajes(tcpClient);
                });

                t.IsBackground = true;
                t.Start();
            }
        }

        public EventHandler<MensajeDTO>? MensajeRecibido; //Lanza a viewmodel.
        private void RecibirMensajes(TcpClient client)
        {
            //Verifica que el cliente este conectado.
            while (client.Connected)
            {
                //Guardo los datos recibidos en la variable ns.
                var ns = client.GetStream(); //Flujo de datos.

                //Verifico que si no recibo ningun dato, descanso medio segundo y prosigo a seguir checando si algun dato ingreso.
                while (client.Available == 0)
                {
                    Thread.Sleep(500);
                }

                //Si hay informacion, entonces:

                //creo un arreglo de bytes del tamaño del total de datos recibidos.
                byte[] buffer = new byte[client.Available];

                //Leo los datos del stream y los guardo en el arreglo.
                ns.Read(buffer, 0, buffer.Length);

                //despues convierto los bytes a una cadena de texto y los guardo en la variable json.
                var json = Encoding.UTF8.GetString(buffer);

                //Deserializo la cadena de texto para que coincida con MensajeDto.
                var mensaje = JsonSerializer.Deserialize<MensajeDTO>(json);

                if(mensaje != null)
                {
                    //Reenviar el mensaje a los otros clientes y a la interfaz grafica con un evento.
                    ReenviarMensaje(client, buffer);
                    MensajeRecibido?.Invoke(this, mensaje);
                }
            }

            //En caso de que no este conectado el cliente, se remueve de la lista.
            clients.Remove(client);
        }

        private void ReenviarMensaje(TcpClient cliente, byte[] mensaje)
        {
            foreach (TcpClient c in clients)
            {
                //Enviar a todos menos el origen y verificar que siga conectado el cliente.
                if(c != cliente && c.Connected)
                {
                    //guardo en una variable el flujo de datos de cada cliente.
                    var ns = c.GetStream();

                    //Los leo y guardo el mensaje en el arreglo.
                    ns.Write(mensaje, 0, mensaje.Length);

                    //Envio los datos sin cerrar el stream.
                    ns.Flush();

                }
            }
        }
    }
}
