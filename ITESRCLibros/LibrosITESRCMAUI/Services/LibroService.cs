using LibrosITESRCMAUI.Models.DTOs;
using LibrosITESRCMAUI.Models.Entities;
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
        Repositories.LibroRepository librosRepository = new();

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
            // var response = await cliente.PostAsJsonAsync<LibrosDTO>("api/libros", dto);
            var response = await cliente.PostAsJsonAsync("api/libros", dto);

            if (response.IsSuccessStatusCode)
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
            try
            {
                //Bajar informacion
                var response = await cliente.GetFromJsonAsync<List<LibrosDTO>>("api/libros");

                //Guardar en base de datos
                if(response != null)
                {
                    foreach (LibrosDTO libro in response)
                    {
                        var entidad = librosRepository.Get(libro.Id ?? 0);

                        if(entidad == null && libro.Eliminado == false) //si no esta en bd local, agregar.
                        {
                            entidad = new()
                            { 
                                Id = libro.Id ?? 0,
                                Autor = libro.Autor,
                                Portada = libro.Portada,
                                Titulo = libro.Titulo
                            
                            };

                            librosRepository.Insert(entidad);  
                        }
                        else 
                        {
                            if(entidad != null)
                            {
                                if (entidad.Eliminado)
                                {
                                    librosRepository.Delete(entidad);
                                }
                                else
                                {
                                    librosRepository.Update(entidad);
                                }
                            }

                        }
                    }
                }

               
            }
            catch
            {

            }
        }
    }
}
