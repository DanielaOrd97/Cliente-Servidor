
//Para acceder a metodos de HTTP.
HttpClient client = new HttpClient
{
    BaseAddress = new Uri("https://localhost:44309/") //Direccion del servidor al que se le enviara la solicitud.
};

Console.Write("Usuario: ");
string username = Console.ReadLine() ?? "";

Console.Write("Contraseña: ");
string password = Console.ReadLine() ?? "";


//solicitud POST al endpoint api/login.
var response = client.PostAsync($"api/login?username={username}&password={password}", null).Result;

//se obtiene el token
var token = response.Content.ReadAsStringAsync().Result;

Console.WriteLine(token);

HttpRequestMessage rm = new();
rm.RequestUri = new Uri(client.BaseAddress + "api/saludos");
rm.Method = HttpMethod.Get;
rm.Headers.Add("Authorization", $"Bearer {token}");

var resp = client.SendAsync(rm).Result;
//resp.EnsureSuccessStatusCode();

if (resp.StatusCode == System.Net.HttpStatusCode.Forbidden)
{
    Console.WriteLine("Acceso Denegado");
}
else
{
    var saludo = resp.Content.ReadAsStringAsync().Result;

    Console.WriteLine(saludo);
}
Console.ReadLine();