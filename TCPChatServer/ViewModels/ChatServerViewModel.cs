using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TCPChatServer.Models;
using TCPChatServer.Services;

namespace TCPChatServer.ViewModels
{
    public class ChatServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatServer Server { get; set; } = new();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ICommand IniciarServerCommand { get; set; }
        public ICommand DetenerCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";
        public ObservableCollection<MensajeDTO> Mensajes { get; set; } = new();


        public ChatServerViewModel()
        {
            //Detecta todas las direcciones IP de mi computadora.

            //Dns.GetHostName(): obtiene el nombre del host de la máquina local.
            //Dns.GetHostAddresses(): Devuelve las direcciones IP asociadas con ese nombre de host.
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName());

            if(direcciones != null)
            {
                //Junta todas las direcciones obtenidas, separandolas por una coma. Solo escoge las IPv4

                IP = string.Join(",", direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                   .Select(x => x.ToString()));
            }

            IniciarServerCommand = new RelayCommand(Iniciar);
            DetenerCommand = new RelayCommand(Detener);
            Server.MensajeRecibido += Server_MensajeRecibido;
        }

        private void Server_MensajeRecibido(object? sender, MensajeDTO e)
        {

            //Dispatcher.CurrentDispatcher.Invoke(() =>
            //{
            //    if (e.Mensaje == "**HELLO")
            //    {
            //        e.Mensaje = $"{e.Origen} se ha conectado.";
            //        Usuarios.Add(e.Origen);
            //    }
            //    else if (e.Mensaje == "**BYE")
            //    {
            //        e.Mensaje = $"{e.Origen} se ha desconectado.";
            //        Usuarios.Remove(e.Origen);
            //    }
            //    Mensajes.Add(e);
            //});

            //Dispatcher ejecuta  operaciones en el hilo de la interfaz de usuario.
            //Se utiliza para programar operaciones que actualizan la interfaz de usuario.

            Application.Current.Dispatcher.Invoke(() =>
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
