using ServidorPostIt.Models;
using ServidorPostIt.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServidorPostIt.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        NotasServer server = new();
        public ObservableCollection<PostIt> Notas { get; set; } = new();
        //public string IP
        //{
        //    get
        //    {
        //        //return string.Join(",", Dns.BeginGetHostAddresses(Dns.GetHostName()))
        //        //    .Where(x => )


        //    }
        //}
        public string IP { get; set; } = "0.0.0.0";

        public NotasViewModel()
        {
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName());

            if (direcciones != null)
            {

                IP = string.Join(",", direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                   .Select(x => x.ToString()));
            }


            server.NotaRecibida += Server_NotaRecibida;
            server.Iniciar();

        }

        Random r = new();
        private void Server_NotaRecibida(object? sender, PostIt e)
        {
            e.Angulo = r.Next(-5, 6);
            Notas.Add(e);
        }
    }
}
