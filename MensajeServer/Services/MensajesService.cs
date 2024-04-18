using MensajeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MensajeServer.Services
{
    public class MensajesService
    {
        HttpListener server = new();

        public MensajesService()
        {
            server.Prefixes.Add("http://*:7002/mensajitos/");
            server.Start();

            new Thread(RecibirPeticiones) { IsBackground = true }.Start();  
        }

        public event EventHandler<Mensaje> MensajeRecibido;
        void RecibirPeticiones()
        {
            while (true)
            {
                var context = server.GetContext(); //peticion

                if(context != null)
                {
                    if (context.Request.QueryString["texto"] != null) //verificar si mandaron el mensaje por querystring
                    {
                        Mensaje mensaje = new()
                        {
                            Texto = context.Request.QueryString["texto"] ?? "",
                            ColorLetra = context.Request.QueryString["letra"] ?? "#000",
                            ColorFondo = context.Request.QueryString["colorfondo"] ?? "#fff"
                        };


                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MensajeRecibido?.Invoke(this, mensaje);
                        });
                    }
                }
            }
        }
    }
}
