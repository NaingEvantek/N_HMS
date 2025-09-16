using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N_HMS.Interfaces;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomservice;

        public RoomController(IRoomService roomService)
        {
            _roomservice = roomService;
        }


        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreateRequest req)
        {
            try
            {
                var room = await _roomservice.CreateRoomAsync(req);
                return Ok(new { message = "Room created successfully", roomId = room.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomUpdateRequest req)
        {
            try
            {
                var room = await _roomservice.UpdateRoomAsync(req);
                if (room == null) return NotFound(new { error = "Room not found" });
                return Ok(new { message = "Room updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("list")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ListRooms([FromBody] QueryRequest req)
        {
            var rooms = await _roomservice.GetAllRoomsAsync(req);

            return Ok(rooms);
        }
    }
}
