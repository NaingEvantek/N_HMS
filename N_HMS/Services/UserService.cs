using System;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.DTO;
using N_HMS.Interfaces;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Services
{
    public class UserService:IUserService
    {
        private readonly N_HMSContext _db;

        public UserService(N_HMSContext db)
        {
            _db = db;
        }

        public async Task<User_Info> CreateUserAsync(string userName, string password, int roleId)
        {
            if (await _db.User_Infos.AnyAsync(u => u.User_Name == userName))
                throw new Exception("Username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

            var user = new User_Info
            {
                User_Name = userName,
                Password_Hash = hashedPassword,
                Role_Id = roleId,
                IsActive = true,
                Created_Date = DateTime.UtcNow
            };

            _db.User_Infos.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User_Info?> UpdateUserAsync(int id, string? userName, string? password, int? roleId, bool? isActive)
        {
            var user = await _db.User_Infos.FindAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(userName))
            {
                if (await _db.User_Infos.AnyAsync(u => u.User_Name == userName && u.Id != id))
                    throw new Exception("Username already exists");
                user.User_Name = userName;
            }

            if (!string.IsNullOrEmpty(password))
                user.Password_Hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

            if (roleId.HasValue)
                user.Role_Id = roleId.Value;

            if (isActive.HasValue)
                user.IsActive = isActive.Value;

            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(int page_index, int page_size, string? sort_by, bool is_desending)
        {
            var query = _db.User_Infos.Include(u => u.Role).AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sort_by))
            {
                query = sort_by.ToLower() switch
                {
                    "username" => is_desending ? query.OrderByDescending(g => g.User_Name) : query.OrderBy(g => g.User_Name),
                    "isactive" => is_desending ? query.OrderByDescending(g => g.IsActive) : query.OrderBy(g => g.IsActive),
                    "roleid" => is_desending ? query.OrderByDescending(g => g.Role_Id) : query.OrderBy(g => g.Role_Id),
                    _ => query.OrderBy(g => g.User_Name) // default sort
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
                .Select(g => new UserDTO
                {
                    Id = g.Id,
                    User_Name = g.User_Name,
                    Role_Id = g.Role_Id,
                    Role_Name = g.Role.Name,
                    IsActive = g.IsActive,
                    Created_Date = g.Created_Date
                })
                .ToListAsync();

            return new PagedResult<UserDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = page_index,
                PageSize = page_size
            };
        }

    }
}
