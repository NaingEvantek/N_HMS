using System;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Models;

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

        public async Task<List<User_Info>> GetAllUsersAsync()
        {
            return await _db.User_Infos.Include(u => u.Role).ToListAsync();
        }

    }
}
