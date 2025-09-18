using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Models;
using N_HMS.Services;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly N_HMSContext _db;
        private readonly ITokenService _tokenService;
        private readonly ILicenseService _licenseService;
        public AuthController(N_HMSContext db, ITokenService tokenService,ILicenseService licenseService)
        {
            _db = db;
            _tokenService = tokenService;
            _licenseService = licenseService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
                return BadRequest(new { error = "Username and password required." });

            var user = await _db.User_Infos
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(u => u.User_Name == req.Username);

            if (user == null || user.IsActive != true)
                return Unauthorized(new { error = "Invalid credentials." });

            var storedHash = user.Password_Hash ?? "";

            bool passwordOk = false;
            try
            {
                passwordOk = BCrypt.Net.BCrypt.Verify(req.Password, storedHash);
            }
            catch
            {
                // stored hash invalid format
                passwordOk = false;
            }

            if (!passwordOk)
                return Unauthorized(new { error = "Invalid credentials." });

            var token = _tokenService.CreateToken(user, user.Role?.Name ?? "User");

            // 2️⃣ Fetch or create license for user
            var license = await _db.Licenses.FirstOrDefaultAsync(l => l.UserId == user.Id && l.IsActive);
            if (license == null)
            {
                license = new License
                {
                    UserId = user.Id,
                    LicenseKey = Guid.NewGuid().ToString(),
                    StartDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                };
                _db.Licenses.Add(license);
                await _db.SaveChangesAsync();
            }

            // 3️ Generate license JWT
            var licenseToken = _licenseService.GenerateLicenseToken(license);

            var resp = new LoginResponse
            {
                Token = token,// User JWT
                X_Token = licenseToken, // License JWT
                Username = user.User_Name ?? "",
                Role = user.Role?.Name ?? ""
            };

            return Ok(resp);
        }
    }
}
