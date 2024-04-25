using LibrosITESRCMAUI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibrosITESRCMAUI.Services
{
    public class LibroService
    {
        //conectar con API externa y deposita datos en la base de datos local.
        HttpClient cliente;

        public LibroService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://libros.itesrc.net/")
            };
        }

        public async Task Agregar(LibrosDTO dto)
        {
            //FORMA DIFICIL
            //var json = JsonSerializer.Serialize(dto);
            //await cliente.PostAsync("api/libros",new StringContent(json, Encoding.UTF8, "application/json"));

            //FORMA FACIL (versiones nuevas) PETICION
            var response = await cliente.PostAsJsonAsync<LibrosDTO>("api/libros", dto);

            if(response.IsSuccessStatusCode)
            {
                await GetLibros(); //Baja los libros de la api a la DB local.
            }
            else
            {
                //var errores = await response.Content.ReadFromJsonAsync<string[]>();
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }

        }

        public async Task GetLibros()
        {

        }
    }
}
