using N_HMS.DTO;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Interfaces
{
    public interface IUserService
    {
        Task<User_Info> CreateUserAsync(string userName, string password, int roleId);
        Task<User_Info?> UpdateUserAsync(int id, string? userName, string? password, int? roleId, bool? isActive);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(QueryRequest req);
    }
}
