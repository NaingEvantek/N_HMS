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
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateFloor([FromBody] FloorCreateRequest req)
        {
            try
            {
                var floor = await _floorService.CreateFloorAsync(req.FloorName);
                return Ok(new { message = "Floor created successfully", floorId = floor.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFloor([FromBody] FloorUpdateRequest req)
        {
            try
            {
                var floor = await _floorService.UpdateFloorAsync(req.Id, req.FloorName);
                if (floor == null) return NotFound(new { error = "Floor not found" });
                return Ok(new { message = "Floor updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("list")]
        public async Task<IActionResult> ListFloors([FromBody]QueryRequest req)
        {
            var floors = await _floorService.GetAllFloorsAsync(req);
           
            return Ok(floors);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("all")]
        public async Task<IActionResult> GetFilterFloors()
        {
            var floors = await _floorService.GetFilterFloorsAsync();

            return Ok(floors);
        }
    }
}
