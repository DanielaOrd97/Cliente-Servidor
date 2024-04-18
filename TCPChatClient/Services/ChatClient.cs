using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TCPChatClient.Models;

namespace TCPChatClient.Services
{
    public class ChatClient
    {
        //Cliente
        TcpClient cliente = null!;

        //varible para nombre del equipo.
        public string equipo = null!;

        public void Conectar(IPAddress ip)
        {

            try
            {
                //Destino (server)
                IPEndPoint ipe = new(ip, 9000);

                //instanciar cliente
                cliente = new();

                //conectar cliente al destino
                cliente.Connect(ipe);

                //asignar el nombre del equipo
                var equipo = Dns.GetHostName();
                //Enviroment

                //al conectarse manda un Hello
                var msg = new MensajeDTO
                {
                    Fecha = DateTime.Now,
                    Mensaje = "**HELLO",
                    Origen = equipo
                };

                EnviarMensaje(msg);
                // cliente.Close();  ???????????????
                RecibirMensaje();
            }
            catch (Exception)
            {

                throw;
            }


           
        }

        public void Desconectar()
        {
               
        }

        public void EnviarMensaje(MensajeDTO msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Mensaje))
            {
                //se serializa el mensaje
                var json = JsonSerializer.Serialize(msg);

                //toma el json y lo convierte en un arreglo de bytes
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                //guardo el flujo de datos
                var ns = cliente.GetStream();

                //se escriben los bytes contenidos en buffer en el flujo de salida
                ns.Write(buffer, 0, buffer.Length);

                //se envian sin cerrar el stream
                ns.Flush();
            }
        }

        public event EventHandler<MensajeDTO>? MensajeRecibido;

        public void RecibirMensaje()
        {

            try
            {
                new Thread(() =>
                {
                    while (cliente.Connected)
                    {
                        //verificar que haya datos
                        if (cliente.Available > 0)
                        {
                            //guardo los datos
                            var ns = cliente.GetStream();

                            //creo un arreglo del tamaño de los datos
                            byte[] buffer = new byte[cliente.Available];

                            //leo los datos y los guardo en el arreglo
                            ns.Read(buffer, 0, buffer.Length);

                            //deserealiza el mensaje
                            var msg = JsonSerializer.Deserialize<MensajeDTO>(Encoding.UTF8.GetString(buffer));

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                if (msg != null)
                                {
                                    MensajeRecibido?.Invoke(this, msg);
                                }
                            });
                        }
                    }

                })
                { IsBackground = true }.Start();
            }
            catch (Exception)
            {

                throw;
            }



          
        }
    }
}
