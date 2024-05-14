using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoticiasProyecto.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration Configuration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GetToken(string username, string role, List<Claim> claims)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();


            var issuer = Configuration.GetSection("Jwt").GetValue<string>("Issuer")??"";
            var audience = Configuration.GetSection("Jwt").GetValue<string>("Audience") ?? "";
            var secret = Configuration.GetSection("Jwt").GetValue<string>("Secret");

            List<Claim> basicas = new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience)
            };
            basicas.AddRange(claims);

            
            

            JwtSecurityToken jwtSecurity = new(issuer,audience,basicas, DateTime.Now, DateTime.Now.AddMinutes(20),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                SecurityAlgorithms.HmacSha256));


            return handler.WriteToken(jwtSecurity);

        }
    }
}
