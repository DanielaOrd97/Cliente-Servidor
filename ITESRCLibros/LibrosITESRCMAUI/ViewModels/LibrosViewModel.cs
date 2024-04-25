using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibrosITESRCMAUI.Models.DTOs;
using LibrosITESRCMAUI.Models.Validators;
using LibrosITESRCMAUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrosITESRCMAUI.ViewModels
{
    public partial class LibrosViewModel:ObservableObject
    {

        LibroService service = new();

        [ObservableProperty]
        private LibrosDTO? libro;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo() //vista
        {
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
                    var resultado = validator.Validate(libro);
                    if (resultado.IsValid)
                    {
                        await service.Agregar(libro);
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
    }
}
