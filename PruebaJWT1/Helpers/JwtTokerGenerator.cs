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
            Claims.Add(new Claim("Rol", "Admin"));
            Claims.Add(new Claim(ClaimTypes.Name, nombre));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iss, "Saludos"));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "prueba"));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(5).ToString())); 

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Claims),
                Issuer = "Saludos",
                Audience = "prueba",
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                NotBefore = DateTime.Now.AddMinutes(-1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ESTAESMILLAVEDECIFRADODELTOKEN2024")),SecurityAlgorithms.HmacSha256)
                //NotBefore = token anticipado
            };

            return handler.CreateEncodedJwt(token);
        }
    }
}
