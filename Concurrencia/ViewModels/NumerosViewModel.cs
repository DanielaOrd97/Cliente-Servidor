using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Concurrencia.ViewModels
{
    public class NumerosViewModel : INotifyPropertyChanged
    {
        public long Suma { get; set; }

        public ICommand SumarCommand { get; set; }

        public NumerosViewModel()
        {
            SumarCommand = new RelayCommand(SumarParallel);
        }

        //SUMAR SINCRONO
        private void SumarSincrono()
        {
            long suma = 0;
            for (int i = 0; i <= 1000000000; i++)
            {
                suma += i;  
            }

            Suma = suma;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
        }

        private async void SumarAsync()
        {
            //_ = Task.Run(() => No espera


            await Task.Run(() =>
               {

                   long suma = 0;
                   for (long i = 0; i <= 1000000000; i++)
                   {
                       suma += i;
                   }
                   Suma = suma;
                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
               });


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
        }

        private void SumarThread()
        {

            //cross thread calls

            Thread hilo = new Thread(() =>
            {
                //Stopwatch s = new();
               //s.start();
                long suma = 0;
                for (long i = 0; i <= 1000000000; i++)
                {
                    suma += i;
                }
                Suma = suma;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));
                //s.stop()
            });
            hilo.IsBackground = true; //segundo plano
            hilo.Start();
        }

        private async void SumarParallel()
        {
            // Task.Run(() => { }
            await Task.Run(() =>
            {
                long suma = 0;
                //Parallel.For(1, 1000000000, (x) =>
                //{
                //    suma += x;
                //});
                //Inicio, num hilos(reparte)
                Parallel.For(1, 10, (x) =>
                {
                    long rango = 1000000000 / 10;
                    long inicial = rango * (x - 1) + 1;

                    for (long i = inicial; i < 1000000000 / 10; i++)
                    {
                        suma += i;
                    }
                });

                Suma = suma;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Suma)));

            });

            //long suma = 0;
            //Parallel.For(1, 1000000000, (x) =>
            //{
            //    suma += x;
            //});
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
