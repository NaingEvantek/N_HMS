using N_HMS.DTO;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Interfaces
{
    public interface IGuestService
    {
        Task<Guest_Info> CreateGuestAsync(string guest_name, string passport_no, int gender_id);
        Task<Guest_Info?> UpdateGuestAsync(int id, string? guest_name, string? passport_no, int? gender_id);
        Task<PagedResult<GuestDTO>> GetAllGuestsAsync(int page_index, int page_size, string? sort_by, bool is_desending);
    }
}
