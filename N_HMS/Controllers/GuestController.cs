using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N_HMS.Interfaces;
using N_HMS.Middlewares;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGuest([FromBody] GuestCreateRequest req)
        {
            try
            {
                var guest = await _guestService.CreateGuestAsync(req.GuestName, req.PassportNo, req.GenderId);
                return Ok(new { message = "User created successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] GuestUpdateRequest req)
        {
            try
            {
                var user = await _guestService.UpdateGuestAsync(req.Id, req.GuestName, req.PassportNo, req.GenderId);
                if (user == null) return NotFound(new { error = "Guest not found" });
                return Ok(new { message = "Guest updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ListGuests([FromBody] QueryRequest req)
        {
            var users = await _guestService.GetAllGuestsAsync(req);
           
            return Ok(users);
        }
    }
}
