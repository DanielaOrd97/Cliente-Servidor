using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibrosITESRCMAUI.Models.DTOs;
using LibrosITESRCMAUI.Models.Entities;
using LibrosITESRCMAUI.Models.Validators;
using LibrosITESRCMAUI.Repositories;
using LibrosITESRCMAUI.Services;
using LibrosITESRCMAUI.WinUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrosITESRCMAUI.ViewModels
{
    public partial class LibrosViewModel:ObservableObject
    {
        LibroRepository libroRepository = new();

        public ObservableCollection<Libro> Libros { get; set; } = new();

        LibroService service = new();

        [ObservableProperty]
        private LibrosDTO? libro;

        [ObservableProperty]
        private string error = "";


        public LibrosViewModel()
        {
            ActualizarLibros();
            App.LibroService.DatosActualizados += LibroService_DatosActualizados;
            //App.LibroService = service;
           // service.DatosActualizados += Service_DatosActualizados;
            //_=service.GetLibros();
        }

        private void LibroService_DatosActualizados()
        {
            ActualizarLibros();
        }

        [RelayCommand]
        public void Nuevo() //vista
        {
            Libro = new();
            Shell.Current.GoToAsync("//Agregar");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Libro = null;
            Error = "";
            Shell.Current.GoToAsync("//Lista");
        }

        LibroValidator validator = new();

        [RelayCommand]
        public async Task Agregar()
        {

            try
            {

                if (Libro != null)
                {
                    var resultado = validator.Validate(Libro);
                    if (resultado.IsValid)
                    {
                        await service.Agregar(Libro);
                        ActualizarLibros();
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {

                Error = ex.Message;
            }
           
        }

        void ActualizarLibros()
        {
            Libros.Clear();
            foreach (var libro in libroRepository.GetAll())
            {
                Libros.Add(libro);
            }
        }

    }
}
