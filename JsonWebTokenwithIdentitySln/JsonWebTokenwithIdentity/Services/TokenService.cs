using JsonWebTokenwithIdentity.Interfaces;
using JsonWebTokenwithIdentity.Models;
using Microsoft.IdentityModel.Tokens;
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
            var secretKey = _config["AppSettings: TokenKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("The token is missing from the configuration");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }
    }
}
