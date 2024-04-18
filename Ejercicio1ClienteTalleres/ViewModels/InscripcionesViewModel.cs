using Ejercicio1ClienteTalleres.Models.Dtos;
using Ejercicio1ClienteTalleres.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ejercicio1ClienteTalleres.ViewModels
{
    public class InscripcionesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public InscripcionDTO datos { get; set; } = new();

        InscripcionesCliente ClienteUdp = new();
        public string IP { get; set; } = "0.0.0.0";
        public ICommand InscribirCommand {  get; set; }

        public InscripcionesViewModel()
        {
            InscribirCommand = new RelayCommand(Inscribir);
        }

        private void Inscribir()
        {
            ClienteUdp.Servidor = IP;
            ClienteUdp.EnviarInscripcion(datos);
        }
    }
}
