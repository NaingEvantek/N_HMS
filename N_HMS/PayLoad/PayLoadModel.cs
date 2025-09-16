namespace N_HMS.PayLoad
{
    public class PayLoadModel
    {
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

        public class FloorCreateRequest
        {
            public string FloorName { get; set; } = null!;
        }

        public class FloorUpdateRequest
        {
            public int Id { get; set; }
            public string FloorName { get; set; } = null!;
        }
    }
}
