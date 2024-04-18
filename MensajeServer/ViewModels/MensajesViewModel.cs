using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensajeServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajeServer.ViewModels
{
    public partial class MensajesViewModel:ObservableObject
    {
        [ObservableProperty]
        private Mensaje? mensaje;

        //[RelayCommand]
        //private void Cancelar() { }

        public MensajesViewModel()
        {
            
        }
    }
}
