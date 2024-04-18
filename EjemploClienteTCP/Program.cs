using System.Net;
using System.Net.Sockets;
using System.Text;

TcpClient cliente = new();

Console.WriteLine("Escribe la ip del servidor: ");
var ip = Console.ReadLine() ?? "127.0.0.1";

var ipe = new IPEndPoint(IPAddress.Parse(ip), 8000); //punto de conexion remoto del servidor al que me quiero conectar.
//Solicita una conexion
cliente.Connect(ipe);

//Se declara una variable cadena para almacenar las entradas del usuario desde la consola.
string cadena;

//Se inicia un bucle while que se ejecutará mientras el usuario siga ingresando líneas desde la consola.
while ((cadena = Console.ReadLine()) != null){
    byte[] buffer = Encoding.UTF8.GetBytes(cadena);
    var ns = cliente.GetStream(); //Obtiene  un medio para enviar y recibir datos a través de esa conexión.

    ns.Write(buffer, 0, buffer.Length); //Se escriben los datos almacenados en el buffer en el flujo de red utilizando este metodo.
    ns.Close(); // Se cierra el flujo de red después de enviar los datos.
}