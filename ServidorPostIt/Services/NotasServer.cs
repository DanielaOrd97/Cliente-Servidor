using ServidorPostIt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace ServidorPostIt.Services
{
    public class NotasServer
    {
        HttpListener server = new();

        //netsh http add urlacl "http://*:12345/notas/" user=Everyone
        public NotasServer()
        {
           // server.Prefixes.Add("http:/*:12345/notas");
            server.Prefixes.Add("http://*:12345/notas/");
        }

        public void Iniciar()
        {
            if (!server.IsListening)
            {
                server.Start();


                new Thread(Escuchar)
                {
                    IsBackground = true
                }.Start();
            }
        }

        public event EventHandler<PostIt>? NotaRecibida;

        public void Escuchar()
        {
            while (true)
            {
                var context = server.GetContext(); //pausa hasta que reciba la peticion.

                var pagina = File.ReadAllText("Assets/index.html");
                var buffer = Encoding.UTF8.GetBytes(pagina);


                if(context.Request.Url != null)
                {
                    
                    if (context.Request.Url.LocalPath == "/notas/")
                    {
                        context.Response.ContentLength64 = buffer.Length;
                        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                    }
                    else if(context.Request.HttpMethod=="POST" && context.Request.Url.LocalPath == "/notas/crear/") //datos del form
                    {
                        byte[] bufferEntrada = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(buffer, 0, bufferEntrada.Length); 
                        string datos = Encoding.UTF8.GetString(bufferEntrada);
                        

                        var diccionario = HttpUtility.ParseQueryString(datos);

                        if(diccionario != null)
                        {
                            //PostIt nota = new()
                            //{
                            //    Titulo = diccionario["titulo"] ?? "",
                            //    Contenido = diccionario["contenido"] ?? "",
                            //    X = double.Parse(diccionario["x"] ?? "0"),
                            //    Y = double.Parse(diccionario["y"] ?? "0"),
                            //    // Remitente = Dns.GetHostByAddress(IPAddress.Parse(context.Request.UserHostAddress));
                            //    Remitente = Dns.GetHostEntry(IPAddress.Parse(context.Request.UserHostAddress)).HostName
                            //};

                            PostIt nota = new()
                            {
                                Titulo = diccionario["titulo"] ?? "",
                                Contenido = diccionario["contenido"] ?? "",
                                X = double.Parse(diccionario["x"] ?? "0"),
                                Y = double.Parse(diccionario["y"] ?? "0"),
                                Remitente = context.Request.RemoteEndPoint.Address.ToString()

                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                NotaRecibida?.Invoke(this, nota);
                            });

                            context.Response.StatusCode = 200;
                            context.Response.Close();
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                        }

                       
                    }
                }

                //http://localhost:12345/notas/

             
                
            }
        }

        public void Detener()
        {
            server.Stop();
        }
    }
}
