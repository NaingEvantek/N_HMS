using N_HMS.DTO;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Interfaces
{
    public interface IFloorService
    {
        Task<Floor_Info> CreateFloorAsync(string floor_name);
        Task<Floor_Info?> UpdateFloorAsync(int id, string? floor_name);
        Task<PagedResult<FloorDTO>> GetAllFloorsAsync(QueryRequest req);
        Task<List<FloorSelectDTO>> GetFilterFloorsAsync();
    }
}
