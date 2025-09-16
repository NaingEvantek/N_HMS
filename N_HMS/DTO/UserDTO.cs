namespace N_HMS.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string User_Name { get; set; } = string.Empty;

        public int? Role_Id { get; set; }

        public string? Role_Name { get; set; } = string.Empty;

        public bool? IsActive { get; set; } = false;

        public DateTime? Created_Date { get; set; }


    }
}
