using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VistaServerTcp.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Cliente> ListaDeImagenes;
        private int indiceActual;

        public ObservableCollection<Cliente> ListaDeImg
        {
            get { return ListaDeImagenes; }
            set
            {
                ListaDeImagenes = value;
                OnPropertyChanged(nameof(ListaDeImagenes));
            }
        }

        public int IndiceActual
        {
            get { return indiceActual; }
            set
            {
                indiceActual = value;
                OnPropertyChanged(nameof(IndiceActual));
            }
        }

        public ICommand AvanzarCommand { get; set; }
        public ICommand RetrocederCommand { get; set; }
        public ServerViewModel()
        {
            AvanzarCommand = new RelayCommand(Avanzar);
            RetrocederCommand = new RelayCommand(Retroceder);

            ListaDeImagenes = new();
            ListaDeImagenes.Add(new Cliente { Nombre = "client 1" });
            ListaDeImagenes.Add(new Cliente { Nombre = "client 2" });
            ListaDeImagenes.Add(new Cliente { Nombre = "client 3" });
        }


        public void Avanzar()
        {
            IndiceActual++;
            if (IndiceActual >= ListaDeImagenes.Count)
                IndiceActual = 0; // Vuelve al principio
        }

        public void Retroceder()
        {
            IndiceActual--;
            if (IndiceActual < 0)
                IndiceActual = ListaDeImagenes.Count - 1; // Vuelve al final
        }

        private void OnPropertyChanged(string v)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));    
        }
    }


    public class Cliente
    {
        public string Nombre { get; set; } = null!;
    }
}
