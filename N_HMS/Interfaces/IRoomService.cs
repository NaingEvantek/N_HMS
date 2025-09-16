using N_HMS.DTO;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Interfaces
{
    public interface IRoomService
    {
        Task<Room_Info> CreateRoomAsync(RoomCreateRequest req);
        Task<Room_Info?> UpdateRoomAsync(RoomUpdateRequest req);
        Task<PagedResult<RoomDTO>> GetAllRoomsAsync(QueryRequest req);
    }
}
