

using System.Net;
using System.Net.Sockets;
using System.Text;

//Clase para hacer clientes y servidores.

//APLICACION CLIENTE

// 1) Se crea una nueva instancia de UdpClient para el cliente.
UdpClient cliente = new();
// 2) Se habilita la difusión de paquetes UDP a través de la red.
cliente.EnableBroadcast = true;

// 3) Se imprime en la consola una solicitud para que el usuario ingrese un mensaje.
Console.WriteLine("Escribe el mensaje a enviar:");

// 4) Se lee la entrada del usuario desde la consola y se asigna a la variable 's'. Si la entrada es nula, se asigna una cadena vacía.
string s = Console.ReadLine()??"";

//Define un endpoint
//Recomendado valores mayores a 5000.
//A donde enviar
//IPEndPoint endPoint = new(IPAddress.Loopback, 5001);

// 5) IPEndPoint crea una direccion ip especial para enviar el mensaje a todos los dispositivos dentro de la red que esten escuchando a traves del puerto 5001.
IPEndPoint endPoint = new(IPAddress.Broadcast, 5001);

//Convertir a binario.
// 6) Se convierte el mensaje ingresado por el usuario en un arreglo de bytes (a binario).
byte[] buffer = Encoding.UTF8.GetBytes(s); //metodo que solo recibe string.

// 7) Se envía el mensaje como un arreglo de bytes al punto de conexión especificado.
cliente.Send(buffer,buffer.Length,endPoint);    