using N_HMS.Models;

namespace N_HMS.Interfaces
{
    public interface IFloorService
    {
        Task<Floor_Info> CreateFloorAsync(string floor_name);
        Task<Floor_Info?> UpdateFloorAsync(int id, string? floor_name);
        Task<List<Floor_Info>> GetAllFloorsAsync();
    }
}
