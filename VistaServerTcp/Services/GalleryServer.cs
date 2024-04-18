using Grpc.Core;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VistaServerTcp.Dtos;
using VistaServerTcp.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VistaServerTcp.Services
{
    public class GalleryServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient?>();  //????
        int num = 0;
        //public event EventHandler<MensajeDto> MensajeRecibido;
        public event EventHandler<PictureDto> ImagenRecibido;
        public event EventHandler<MensajeDto> MensajeRecibido;
        public void Iniciar()
        {
            server = new(new IPEndPoint(IPAddress.Any, 9630));
            server.Start();
            new Thread(Escuchar) { IsBackground = true }.Start();

        }

        private void Escuchar()
        {
            while (server.Server.IsBound)
            {
                var tcpClient = server.AcceptTcpClient();
                clients.Add(tcpClient); ////??

                Thread t = new(() =>
                {
                    RecibirMensajes(tcpClient);
                });
                t.IsBackground = true;
                t.Start();
            }
        }



        private void RecibirMensajes(TcpClient cliente)
        {
            while (cliente.Connected)
            {
                var ns = cliente.GetStream();

                while (cliente.Available == 0)
                {
                    Thread.Sleep(500);
                }

                byte[] buffer = new byte[cliente.Available];
                ns.Read(buffer, 0, buffer.Length);

                string json = Encoding.UTF8.GetString(buffer);

                var mensaje1 = JsonSerializer.Deserialize<PictureDto>(json);
                var mensaje = JsonSerializer.Deserialize<MensajeDto>(json);

                if (mensaje1.Image != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ImagenRecibido?.Invoke(this, mensaje1);
                    });
                }


                if (mensaje != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MensajeRecibido?.Invoke(this, mensaje);
                    });
                }
            }




            //    }




        }


    }
}


