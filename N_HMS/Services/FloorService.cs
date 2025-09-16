using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Models;

namespace N_HMS.Services
{
    public class FloorService:IFloorService
    {
        private readonly N_HMSContext _db;

        public FloorService(N_HMSContext db)
        {
            _db = db;
        }

        public async Task<Floor_Info> CreateFloorAsync(string floor_name)
        {
            if (await _db.Floor_Infos.AnyAsync(f=>f.Name==floor_name))
                throw new Exception("Username already exists");

         
            var floor = new Floor_Info
            {
                Name = floor_name,
                Modified_Date = DateTime.UtcNow
            };

            _db.Floor_Infos.Add(floor);
            await _db.SaveChangesAsync();
            return floor;
        }

        public async Task<Floor_Info?> UpdateFloorAsync(int id, string? floor_name)
        {
            var floor = await _db.Floor_Infos.FindAsync(id);
            if (floor == null) return null;

            if (!string.IsNullOrEmpty(floor_name))
            {
                if (await _db.Floor_Infos.AnyAsync(f => f.Name == floor_name && f.Id != id))
                    throw new Exception("Floor name already exists");
                floor.Name = floor_name;
            }

            await _db.SaveChangesAsync();
            return floor;
        }

        public async Task<List<Floor_Info>> GetAllFloorsAsync()
        {
            return await _db.Floor_Infos.ToListAsync();
        }
    }
}
