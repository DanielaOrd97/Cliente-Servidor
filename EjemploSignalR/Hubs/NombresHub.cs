using Microsoft.AspNet.SignalR;

namespace EjemploSignalR.Hubs
{
    public class NombresHub:Hub
    {
        public async void RecibirNombre( string nombre)
        {
            //Hacer algo con el nombre(guardar en base de datos..)
            await Clients.All.SendAsync("NombreNuevo", nombre);
        }
    }
}
