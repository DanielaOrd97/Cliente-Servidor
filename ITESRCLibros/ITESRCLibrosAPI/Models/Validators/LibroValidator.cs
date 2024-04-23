﻿using FluentValidation;
using ITESRCLibrosAPI.Models.Dtos;

namespace ITESRCLibrosAPI.Models.Validators
{
    public class LibroValidator:AbstractValidator<LibroDTO>
    {
        public LibroValidator()
        {
            //REGLAS DE VALIDACION
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo no debe estar vacio.");
            RuleFor(x => x.Autor).NotEmpty().WithMessage("El autor no debe estar vacio.");
            RuleFor(x => x.Portada).NotEmpty().WithMessage("La imagen de portada no debe estar vacia.");
            RuleFor(x => x.Portada).Must(ValidarUrl).WithMessage("Escriba una direccion URL de una imagen JPEG."); //Must es crear tu propia regla.
        }

        private bool ValidarUrl(string url)
        {
            return url.StartsWith("https://") && url.EndsWith(".jpg");
        }
    }
}