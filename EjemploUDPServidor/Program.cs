// See https://aka.ms/new-console-template for more information

//SEVIDOR UDP

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

//UdpClient server = new();



//IPAddress.Loopback
//A donde recibir
//Bind: No continua hasta que reciba datos.


// 1) IPAddress.Any, lo que indica que el servidor escuchará en todas las interfaces de red disponibles en el equipo. El puerto se establece en 5001.
IPEndPoint endPoint = new(IPAddress.Any, 5001);

// 2) Se crea una nueva instancia de UdpClient para el servidor y endPoint configura el servidor para que escuche en la dirección IP y el puerto especificados.
UdpClient server = new(endPoint);

// 3) permite al servidor recibir mensajes enviados a la dirección IP de difusión.
server.EnableBroadcast = true;

// 4) Se inicia un bucle while infinito para que el servidor pueda recibir mensajes continuamente.
while (true)
{
    //Recibir
    //ref separa el endpoint para no ser usado por los demas.

    // 5) Con e metodo Receive se espera un paquete UDP.
    byte[] buffer = server.Receive(ref endPoint);

    // 6) Los datos recibidos se convierten a string.
    string mensaje = Encoding.UTF8.GetString(buffer); //convierte letra en formato de 8 bits.

    // 7) Se imprime el mensaje.
    Console.WriteLine("Mensaje recibido: " + mensaje);
    Console.ReadLine();
}

