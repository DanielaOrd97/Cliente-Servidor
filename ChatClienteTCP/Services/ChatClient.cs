using ChatClienteTCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
//using static System.Net.Mime.MediaTypeNames;

namespace ChatClienteTCP.Services
{
    public class ChatClient
    {
        TcpClient cliente = null!;
        public string equipo = null!; //**
        public void Conectar(IPAddress ip)
        {
           
            IPEndPoint ipe = new(ip, 9000);
            cliente = new();
            cliente.Connect(ipe);

            var equipo = Dns.GetHostName();


            var msg = new MensajeDTO
            {
                Fecha = DateTime.Now,
                Mensaje = "**HELLO",
                Origen = equipo
            };


            EnviarMensaje(msg);
            cliente.Close();

        }


        public event EventHandler<MensajeDTO>? MensajeRecibido; 

        public void Desconectar()
        {
               
        }

        private void RecibirMensaje()
        {
            new Thread(() =>
            {
                //try catch
                while (cliente.Connected)
                {
                    if(cliente.Available > 0)
                    {
                        var ns = cliente.GetStream();
                        byte[] buffer = new byte[cliente.Available];
                        ns.Read(buffer,0,buffer.Length);

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
            {IsBackground = true}.Start();
        }

        public void EnviarMensaje(MensajeDTO mensaje)
        {
             if (string.IsNullOrWhiteSpace(mensaje.Mensaje))
             {
                var json = JsonSerializer.Serialize(mensaje);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                var ns = cliente.GetStream();
                ns.Write(buffer, 0, buffer.Length);
                ns.Flush();
             }
        }
    }
}
