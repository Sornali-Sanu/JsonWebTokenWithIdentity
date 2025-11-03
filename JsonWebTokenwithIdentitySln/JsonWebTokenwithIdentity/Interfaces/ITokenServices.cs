using JsonWebTokenwithIdentity.Models;

namespace JsonWebTokenwithIdentity.Interfaces
{
    public interface ITokenServices
    {
        string CreateToken(ApplicationUser user);

    }
}
