using N_HMS.DTO;
using N_HMS.Models;

namespace N_HMS.PayLoad
{
    public class PayLoadModel
    {
        #region Auth

        public class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class LoginResponse
        {
            public string Token { get; set; } = null!;
        //    public string X_Token { get; set; } = null!;
            public string Username { get; set; } = null!;
            public string Role { get; set; } = null!;
        }

        #endregion

        #region User

        public class UserCreateRequest
        {
            public string UserName { get; set; } = null!;
            public string Password { get; set; } = null!;
            public int RoleId { get; set; }
        }

        public class UserUpdateRequest
        {
            public int Id { get; set; }
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public int? RoleId { get; set; }
            public bool? IsActive { get; set; }
        }

        #endregion

        #region Floor

        public class FloorCreateRequest
        {
            public string FloorName { get; set; } = null!;
        }

        public class FloorUpdateRequest
        {
            public int Id { get; set; }
            public string FloorName { get; set; } = null!;
        }

        #endregion

        #region Guest

        public class GuestCreateRequest
        {
            public string GuestName { get; set; } = null!;
            public string PassportNo { get; set; } = null!;
            public int GenderId { get; set; }
        }

        public class GuestUpdateRequest
        {
            public int Id { get; set; }
            public string? GuestName { get; set; } = null!;
            public string? PassportNo { get; set; } = null!;
            public int? GenderId { get; set; }
        }

        #endregion

        #region Room

        public class RoomCreateRequest
        {
            public string RoomName { get; set; } = null!;
            public int FloorId { get; set; } 
            public int RoomTypeId { get; set; }
            public decimal PricePerDay { get; set; }
            public int CurrencyTypeId { get; set; }
            public int RoomCapacityAdult { get; set; }
            public int RoomCapacityChild { get; set; }
        }

        public class RoomUpdateRequest
        {
            public int Id { get; set; }
            public string? RoomName { get; set; } = null!;
            public int? FloorId { get; set; }
            public int? RoomTypeId { get; set; }
            public int? RoomStatusId { get; set; }
            public decimal? PricePerDay { get; set; }
            public int? CurrencyTypeId { get; set; }
            public int? RoomCapacityAdult { get; set; }
            public int? RoomCapacityChild { get; set; }
        }

        public class RoomQueryResponse
        {
            public int count { get; set; }
            public string next { get; set; } = "";
            public List<RoomWithBookingDto> results { get; set; } = new();

        }

        public class RoomQueryRequest
        {
            public int? floorId { get; set; }
            public int? roomtypeId { get; set; }
            public int? roomstatusId { get; set; }
            public string? search { get; set; }
            public string? orderby { get; set; } = "";
            public int? page { get; set; } = 1;
        }

        public class RoomCheckInRequest
        {
            public int roomId { get; set; }
            public int paymentStatusId { get; set; }
            public int numOfGuests { get; set; }
            public int numOfDays { get; set; }
            public decimal totalAmount { get; set; }
            public decimal paidAmount { get; set; }
            public List<RoomCheckInGuest> guests { get; set; } = new();
        }

        public class RoomCheckInGuest
        {
            public string GuestName { get; set; } = "";

            public string PassportNo { get; set; } = "";

            public int? GenderId { get; set; }
        }
        #endregion

        #region Common
        public class QueryRequest
        {
            public int PageIndex { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public string? SortBy { get; set; }
            public bool IsDescending { get; set; } = false;
        }

        public class PagedResult<T>
        {
            public List<T> Items { get; set; } = new();
            public int TotalCount { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }

        #endregion
    }
}
