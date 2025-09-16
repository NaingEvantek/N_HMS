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
