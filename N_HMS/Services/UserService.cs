using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(QueryRequest req)
        {
            var query = _db.User_Infos
                        .Include(u => u.Role)
                        .AsQueryable();

            // Sorting map
            var sortMap = new Dictionary<string, Expression<Func<User_Info, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["username"] = u => u.User_Name,
                ["isactive"] = u => u.IsActive??false,
                ["roleid"] = u => u.Role_Id??0
            };

            // Apply sorting
            if (!string.IsNullOrEmpty(req.SortBy) && sortMap.TryGetValue(req.SortBy, out var sortExpr))
            {
                query = req.IsDescending ? query.OrderByDescending(sortExpr) : query.OrderBy(sortExpr);
            }
            else
            {
                query = query.OrderBy(u => u.Id); // default
            }

            // Total count
            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    User_Name = u.User_Name,
                    Role_Id = u.Role_Id,
                    Role_Name = u.Role.Name,
                    IsActive = u.IsActive,
                    Created_Date = u.Created_Date
                })
                .ToListAsync();

            var items = data.Select((u, index) =>
            {
                u.No = ((req.PageIndex - 1) * req.PageSize) + index + 1;
                return u;
            }).ToList();

            return new PagedResult<UserDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = req.PageIndex,
                PageSize = req.PageSize
            };
        }

    }
}
