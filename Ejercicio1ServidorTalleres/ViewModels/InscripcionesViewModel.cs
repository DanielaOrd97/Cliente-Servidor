using Ejercicio1ServidorTalleres.Models;
using Ejercicio1ServidorTalleres.Models.Dtos;
using Ejercicio1ServidorTalleres.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ejercicio1ServidorTalleres.ViewModels
{
    public class InscripcionesViewModel : INotifyPropertyChanged
    {

        public string IP { get; set; } = "0.0.0.0";
        public ObservableCollection<Taller> Talleres { get; set; } = new();

        List<Taller> talleres = new(); //Datos persistentes(se guardan).

        InscripcionesServer servidor = new();

        public void Cargar()
        {

            if (File.Exists("talleres.json"))
            {
                var archivo = File.OpenRead("talleres.json");
                talleres = JsonSerializer.Deserialize<List<Taller>>(archivo) ?? new()
                {
                new Taller(){NombreTaller = "Canto", ListaAlumnos = new List<Alumno>()},
                new Taller(){NombreTaller = "Baile", ListaAlumnos = new List<Alumno>()}
                };
                archivo.Close();
            }
            else
            {
                //talleres = JsonSerializer.Deserialize<List<Taller>>(archivo) ?? new()
                //{
                //new Taller(){NombreTaller = "Canto", ListaAlumnos = new List<Alumno>()},
                //new Taller(){NombreTaller = "Baile", ListaAlumnos = new List<Alumno>()}
                //};

                talleres =  new()
                {
                new Taller(){NombreTaller = "Canto", ListaAlumnos = new List<Alumno>()},
                new Taller(){NombreTaller = "Baile", ListaAlumnos = new List<Alumno>()}
                };
            }
          
        }

        public void Guardar()
        {
            var archivo = File.Create("talleres.json");
            JsonSerializer.Serialize(archivo, talleres);
            archivo.Close();
        }
        public void Actualizar()
        {
            Talleres.Clear();
            foreach (var item in talleres)
            {
                Talleres.Add(item);
            }
        }

        public InscripcionesViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            IP = ips.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).
                Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";
            Cargar();
            Actualizar();
            servidor.InscripcionRealizada += Servidor_InscripcionRealizada;
            
        }

        private void Servidor_InscripcionRealizada(object? sender, InscripcionDto e)
        {
            if(e.Taller == "Ninguno")
            {
                foreach (var item in Talleres)
                {
                    var alumno = item.ListaAlumnos.FirstOrDefault(x => x.Nombre == e.Nombre);
                    if(alumno!= null)
                    {
                        item.ListaAlumnos.Remove(alumno);
                    }
                }
            }
            else
            {
                var taller = talleres.FirstOrDefault(x => x.NombreTaller == e.Taller);
                if(taller != null)
                {
                    taller.ListaAlumnos.Add(new Alumno { Nombre = e.Nombre });
                }
            }

            Guardar();
            Actualizar();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
