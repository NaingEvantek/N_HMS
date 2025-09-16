using System.Linq.Expressions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.DTO;
using N_HMS.Interfaces;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Services
{
    public class GuestService:IGuestService
    {
        private readonly N_HMSContext _db;

        public GuestService(N_HMSContext db)
        {
            _db = db;
        }

        public async Task<Guest_Info> CreateGuestAsync(string guest_name, string passport_no, int gender_id)
        {
            if (await _db.Guest_Infos.AnyAsync(u => u.Name == guest_name))
                throw new Exception("Guest name already exists");

            var guest = new Guest_Info
            {
                Name = guest_name,
                Passport_No = passport_no,
                Gender_Id = gender_id,
                Created_Date = DateTime.UtcNow 
            };

            _db.Guest_Infos.Add(guest);
            await _db.SaveChangesAsync();
            return guest;
        }

        public async Task<Guest_Info?> UpdateGuestAsync(int id, string? guest_name, string? passport_no, int? gender_id)
        {
            var guest = await _db.Guest_Infos.FindAsync(id);
            if (guest == null) return null;

            if (!string.IsNullOrEmpty(guest_name))
            {
                if (await _db.User_Infos.AnyAsync(u => u.User_Name == guest_name && u.Id != id))
                    throw new Exception("Guest Name already exists");
                guest.Name = guest_name;               
            }


            if (string.IsNullOrEmpty(passport_no))
                guest.Passport_No = passport_no;

            if (gender_id.HasValue)
                guest.Gender_Id = gender_id.Value;

            await _db.SaveChangesAsync();
            return guest;
        }

        public async Task<PagedResult<GuestDTO>> GetAllGuestsAsync(QueryRequest req)
        {
            var query = _db.Guest_Infos
          .Include(g => g.Gender)
          .AsQueryable();

            // Sorting map
            var sortMap = new Dictionary<string, Expression<Func<Guest_Info, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["guestname"] = g => g.Name,
                ["passportno"] = g => g.Passport_No,
                ["genderid"] = g => g.Gender_Id??0
            };

            // Apply sorting
            if (!string.IsNullOrEmpty(req.SortBy) && sortMap.TryGetValue(req.SortBy, out var sortExpr))
            {
                query = req.IsDescending ? query.OrderByDescending(sortExpr) : query.OrderBy(sortExpr);
            }
            else
            {
                query = query.OrderBy(g => g.Name); // default
            }

            // Total count
            var totalCount = await query.CountAsync();

            // Pagination + projection
            var data = await query
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(g => new GuestDTO
                {
                    Id = g.Id,
                    GuestName = g.Name,
                    PassportNo = g.Passport_No,
                    GenderId = g.Gender_Id,
                    GenderName = g.Gender.Name,
                    CreatedDate = g.Created_Date
                })
                .ToListAsync();

            // Add row numbers in memory
            var items = data.Select((g, index) =>
            {
                g.No = ((req.PageIndex - 1) * req.PageSize) + index + 1;
                return g;
            }).ToList();

            return new PagedResult<GuestDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = req.PageIndex,
                PageSize = req.PageSize
            };
        }
    }
}
