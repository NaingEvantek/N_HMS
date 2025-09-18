using N_HMS.Models;

namespace N_HMS.Interfaces
{
    public interface ILicenseService
    {
        string GenerateLicenseToken(License license);
        License? ValidateLicenseToken(string token);
    }
}
