using Microsoft.AspNetCore.SignalR;

namespace EjemploSignalR.Hubs
{
    public class NombresHub:Hub
    {
        public async void AgregarNombre( string nombre)
        {
            //Hacer algo con el nombre(guardar en base de datos..)
            await Clients.All.SendAsync("NombreNuevo", nombre);
        }

        public async void EliminarNombre(string nombre)
        {
            await Clients.All.SendAsync("NombreBorrado", nombre);

        }
    }
}
