using System.Globalization;
using System.Linq.Expressions;
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

        public async Task<PagedResult<FloorDTO>> GetAllFloorsAsync(QueryRequest req)
        {
            var query = _db.Floor_Infos.AsQueryable();

            // Sorting map
            var sortMap = new Dictionary<string, Expression<Func<Floor_Info, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["floorname"] = f => f.Name
            };

            // Apply sorting
            if (!string.IsNullOrEmpty(req.SortBy) && sortMap.TryGetValue(req.SortBy, out var sortExpr))
            {
                query = req.IsDescending ? query.OrderByDescending(sortExpr) : query.OrderBy(sortExpr);
            }
            else
            {
                query = query.OrderBy(f => f.Id); // default
            }

            var totalCount = await query.CountAsync();

            // Get page data
            var data = await query
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(f => new FloorDTO
                {
                    Id = f.Id,
                    FloorName = f.Name,
                    ModifiedDate = f.Modified_Date ?? DateTime.MinValue
                })
                .ToListAsync();

            // Add row numbers in memory
            var items = data.Select((f, index) =>
            {
                f.No = ((req.PageIndex - 1) * req.PageSize) + index + 1;
                return f;
            }).ToList();

            return new PagedResult<FloorDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = req.PageIndex,
                PageSize = req.PageSize
            };
        }
    }
}
