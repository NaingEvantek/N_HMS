using N_HMS.Models;

namespace N_HMS.Interfaces
{
    public interface IUserService
    {
        Task<User_Info> CreateUserAsync(string userName, string password, int roleId);
        Task<User_Info?> UpdateUserAsync(int id, string? userName, string? password, int? roleId, bool? isActive);
        Task<List<User_Info>> GetAllUsersAsync();
    }
}
