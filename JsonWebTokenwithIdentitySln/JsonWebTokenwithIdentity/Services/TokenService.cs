using JsonWebTokenwithIdentity.Interfaces;
using JsonWebTokenwithIdentity.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JsonWebTokenwithIdentity.Services
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config=config ?? throw new ArgumentNullException(nameof(config));
        }
        public string CreateToken(ApplicationUser user)
        {
            var secretKey = _config["AppSettings:TokenKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("The token is missing from the configuration");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                
                issuer:null,
                audience:null,
                claims:claims,
                expires:DateTime.UtcNow.AddMinutes(60),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
