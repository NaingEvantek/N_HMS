using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.Interfaces;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly N_HMSContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(N_HMSContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
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

            var resp = new LoginResponse
            {
                Token = token,
                Username = user.User_Name ?? "",
                Role = user.Role?.Name ?? ""
            };

            return Ok(resp);
        }
    }
}
