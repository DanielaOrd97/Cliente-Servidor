using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PruebaJWT1.Helpers
{
    public class JwtTokerGenerator
    {
        public string GetToken(string nombre)
        {
            List<Claim> Claims = new();


            if (nombre == "Daniela")
            {
                Claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else
            {
                Claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            //un token debe durar maximo 20 mins, cada vez que expire un token tiene que generar otro para volver a tener acceso a los metodos
            //uno de las formas de manejar esto es un metodo que regenere un token o tener 2 tokens, uno con una duracion mucho mayor

            Claims.Add(new Claim(ClaimTypes.Name, nombre));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iss, "Saludos"));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "prueba"));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(5).ToString())); 

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Claims),
                Issuer = "Saludos",
                Audience = "prueba",
                IssuedAt = DateTime.Now,
                Expires = DateTime.UtcNow.AddMinutes(5),
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ESTAESMILLAVEDECIFRADODELTOKEN2024")),SecurityAlgorithms.HmacSha256)
                //NotBefore = token anticipado
            };

            return handler.CreateEncodedJwt(token);
        }
    }
}
