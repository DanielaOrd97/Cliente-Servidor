using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VistaServerTcp.Dtos;
using VistaServerTcp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VistaServerTcp.ViewModels
{
    public class ServidorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public GalleryServer server { get; set; } = new();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ICommand IniciarServerCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public ObservableCollection<MensajeDto> Mensajes { get; set; } = new();
        public ObservableCollection<PictureDto>ImagenesUsuarios {  get; set; } = new(); 
        int num = 0;
        public ServidorViewModel()
        {
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName());
            if (direcciones != null)
            {
                IP = string.Join(",", direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x => x.ToString()));
            }
            IniciarServerCommand = new RelayCommand(IniciarServer);
            //server.MensajeRecibido += Server_MensajeRecibido;
            server.MensajeRecibido += Server_MensajeRecibido;
            server.ImagenRecibido += Server_ImagenRecibido;
            IniciarServer();
        }

        private void Server_ImagenRecibido(object? sender, PictureDto e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Image != null && e.Autor != null)
                {

                    string base64Image = e.Image;
                    byte[] imageBytes = Convert.FromBase64String(base64Image);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        System.Drawing.Image imagen = System.Drawing.Image.FromStream(ms);
                        string archivo = $"imagen{num}.png";
                        string carpetaUsuarios = "UsersImages";
                        string rutaCarpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, carpetaUsuarios);

                        if (!Directory.Exists(rutaCarpeta))
                        {
                            Directory.CreateDirectory(rutaCarpeta);
                        }

                        string rutaCompleta = Path.Combine(rutaCarpeta, archivo);

                        imagen.Save(rutaCompleta, System.Drawing.Imaging.ImageFormat.Png);

                        BitmapImage bitmapimage = new();
                        bitmapimage = new BitmapImage(new Uri(rutaCompleta));
                        e.img = bitmapimage;
                    }


                    num++;
                    ImagenesUsuarios.Add(e);
                    //BitmapImage bitmapimage = new();
                    //bitmapimage = new BitmapImage(new Uri("C:\\Users\\danie\\Documents\\Octavo\\VistaServerTcp\\UsersImages\\imagen0.png"));
                    //e.img = bitmapimage;
                    //ImagenesUsuarios.Add(e);
                }


            });
        }

        private void Server_MensajeRecibido(object? sender, MensajeDto e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Message == "**HELLO")
                {
                    e.Message = $"{e.Name} se ha conectado";
                    Usuarios.Add(e.Name);
                }
                else if (e.Message == "**BYE")
                {
                    e.Message = $"{e.Name} se ha desconectado";
                    Usuarios.Remove(e.Name);
                }
                Mensajes.Add(e);
            });
        }

        public void IniciarServer()
        {
            server.Iniciar();
        }
    }
}
