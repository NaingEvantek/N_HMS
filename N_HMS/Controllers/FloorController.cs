using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N_HMS.Interfaces;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }


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

        [HttpGet("list")]
        public async Task<IActionResult> ListFloors([FromBody]QueryRequest req)
        {
            var floors = await _floorService.GetAllFloorsAsync(req);
           
            return Ok(floors);
        }
    }
}
