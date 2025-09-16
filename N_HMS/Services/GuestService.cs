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
                    throw new Exception("Username already exists");
                guest.Name = guest_name;               
            }


            if (string.IsNullOrEmpty(passport_no))
                guest.Passport_No = passport_no;

            if (gender_id.HasValue)
                guest.Gender_Id = gender_id.Value;

            await _db.SaveChangesAsync();
            return guest;
        }

        public async Task<PagedResult<GuestDTO>> GetAllGuestsAsync(int page_index,int page_size,string? sort_by,bool is_desending)
        {
            var query = _db.Guest_Infos.Include(u => u.Gender).AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sort_by))
            {
                query = sort_by.ToLower() switch
                {
                    "guestname" => is_desending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name),
                    "passportno" => is_desending ? query.OrderByDescending(g => g.Passport_No) : query.OrderBy(g => g.Passport_No),
                    "genderid" => is_desending ? query.OrderByDescending(g => g.Gender_Id) : query.OrderBy(g => g.Gender_Id),
                    _ => query.OrderBy(g => g.Name) // default sort
                };
            }
            else
            {
                query = query.OrderBy(g => g.Name); // default if SortBy is null
            }

            // Total count
            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((page_index - 1) * page_size)
                .Take(page_size)
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

            return new PagedResult<GuestDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex =page_index,
                PageSize = page_size
            };
        }
    }
}
