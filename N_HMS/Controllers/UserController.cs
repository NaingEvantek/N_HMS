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
    [Authorize(Roles = "Admin")]
    [LicenseRequired]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest req)
        {
            try
            {
                var user = await _userService.CreateUserAsync(req.UserName, req.Password, req.RoleId);
                return Ok(new { message = "User created successfully", userId = user.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest req)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(req.Id, req.UserName, req.Password, req.RoleId, req.IsActive);
                if (user == null) return NotFound(new { error = "User not found" });
                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListUsers([FromBody] QueryRequest req)
        {
            var users = await _userService.GetAllUsersAsync(req);
            return Ok(users);
        }
    }
}
