using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Models;

namespace N_HMS.Services
{
    public class LicenseService:ILicenseService
    {
        private readonly N_HMSContext _db;
        private readonly string _secretKey;

        public LicenseService(N_HMSContext db, IConfiguration config)
        {
            _db = db;
            _secretKey = config["License:SecretKey"]?? throw new Exception("License secret key is missing!"); 
        }

        public string GenerateLicenseToken(License license)
        {
            var claims = new List<Claim>
        {
            new Claim("userId", license.UserId.ToString()),
            new Claim("licenseKey", license.LicenseKey.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: license.ExpiryDate,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public License? ValidateLicenseToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true
                }, out var validatedToken);

                var licenseKey = principal.FindFirst("licenseKey")?.Value;
                if (licenseKey == null) return null;

                return _db.Licenses.FirstOrDefault(l => l.LicenseKey.ToString() == licenseKey && l.IsActive);
            }
            catch
            {
                return null;
            }
        }
    }
}
