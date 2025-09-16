using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.DTO;
using N_HMS.Interfaces;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

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

        public async Task<PagedResult<FloorDTO>> GetAllFloorsAsync(int page_index, int page_size, string? sort_by, bool is_desending)
        {
            var query = _db.Floor_Infos.AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sort_by))
            {
                query = sort_by.ToLower() switch
                {
                    "floorname" => is_desending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name),
                    _ => query.OrderBy(g => g.Id) // default sort
                };
            }
            else
            {
                query = query.OrderBy(g => g.Id); // default if SortBy is null
            }

            // Total count
            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((page_index - 1) * page_size)
                .Take(page_size)
                .Select((g,index) => new FloorDTO
                {
                    No = ((page_index - 1) * page_size) + index + 1,
                    Id = g.Id,
                    FloorName = g.Name,
                    ModifiedDate = g.Modified_Date ?? DateTime.MinValue
                })
                .ToListAsync();

            return new PagedResult<FloorDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = page_index,
                PageSize = page_size
            };
        }
    }
}
