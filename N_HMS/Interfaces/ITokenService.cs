using N_HMS.Models;

namespace N_HMS.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User_Info user, string roleName);
    }
}
