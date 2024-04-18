//Iniciamos un servidor TCP
using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new(IPAddress.Any,8000); //Escucha en todas las interfaces de red locales en el puerto 8000.
server.Start(); // Se inicia el servidor para que comience a escuchar las conexiones entrantes en el puerto especificado.

//bucle infinito que espera continuamente nuevas conexiones de clientes.
while (true)
{
    TcpClient client = server.AcceptTcpClient(); //PUERTO RANDOM  El servidor espera y acepta una nueva conexión de cliente. 


    //Se imprime en la consola la información sobre el cliente recién aceptado, incluida su dirección IP y número de puerto.
    Console.WriteLine("Cliente aceptado: " + client.Client.RemoteEndPoint?.ToString());

    Thread hilo = new(() =>
    {
        AtenderCliente(client);
    });


    //hilo secundario que no impedirá que la aplicación se cierre si todos los otros hilos finalizan.
    hilo.IsBackground = true;

    //Se inicia el hilo para comenzar a atender al cliente.
    hilo.Start();
}

void AtenderCliente(TcpClient client)
{
    while (true)
    {
        if (client.Available > 0) //verifica si hay datos disponibles para ser leídos.
        {
            var ns = client.GetStream(); //Se obtiene el flujo de datos asociado al cliente.
            byte[] buffer = new byte[client.Available]; //Se crea un búfer de bytes para almacenar los datos recibidos del cliente.
            ns.Read(buffer, 0, buffer.Length); // Se leen los datos del flujo de datos del cliente y se almacenan en el buffer.

            //Se imprime la direccion IP y el num de puerto del cliente y los datos recibidos convertidos a una cadena.
            Console.WriteLine(client.Client.RemoteEndPoint.ToString() + ":" + Encoding.UTF8.GetString(buffer));

            //Flushing
        }
    }
}