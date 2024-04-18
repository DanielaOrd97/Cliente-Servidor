using ChatTCP.Models;
using ChatTCP.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatTCP.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatServer Server { get; set; } = new();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ICommand IniciarServerCommand { get; set; }
        public ICommand DetenerCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public ObservableCollection<MensajeDTO> Mensajes { get; set; } = new();

        public ChatViewModel()
        {
            //Detectar IP
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName()); //Todas las direcciones de mi compu.
            
            if(direcciones != null)
            {
                IP = string.Join(",", direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(x => x.ToString()));
            }


            IniciarServerCommand = new RelayCommand(Iniciar);
            DetenerCommand = new RelayCommand(Detener);

            Server.MensajeRecibido += Server_MensajeRecibido;

        }

        private void Server_MensajeRecibido(object? sender, MensajeDTO e)
        {

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Mensaje == "**HELLO")
                {
                    e.Mensaje = $"{e.Origen} se ha conectado.";
                    Usuarios.Add(e.Origen);
                }
                else if (e.Mensaje == "**BYE")
                {
                    e.Mensaje = $"{e.Origen} se ha desconectado.";
                    Usuarios.Remove(e.Origen);
                }

                Mensajes.Add(e);
            });


        }

        private void Detener()
        {
            Mensajes.Clear();
            Usuarios.Clear();
            Server.Detener();
        }

        private void Iniciar()
        {
            Server.Iniciar();
        }
    }
}
