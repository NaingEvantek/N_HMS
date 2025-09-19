using N_HMS.DTO;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomTypeSelectDTO>> GetRoomTypesAsync();
        Task<Room_Info> CreateRoomAsync(RoomCreateRequest req);
        Task<Room_Info?> UpdateRoomAsync(RoomUpdateRequest req);
        Task<RoomQueryResponse> SearchRoomAsync(RoomQueryRequest req);
        Task<PagedResult<RoomDTO>> GetAllRoomsAsync(QueryRequest req);
        Task CheckInAsync(RoomCheckInRequest req);
        Task CheckOutAsync(int roomId);
        Task CompleteRoomCleaningAsync(int roomId);
    }
}
