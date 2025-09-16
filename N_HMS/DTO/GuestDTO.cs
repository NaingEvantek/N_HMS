namespace N_HMS.DTO
{
    public class GuestDTO
    {
        public int No { get; set; }
        public int Id { get; set; }
        public string GuestName { get; set; } = null!;
        public string PassportNo { get; set; } = null!;
        public int? GenderId { get; set; }
        public string GenderName { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
    }
}
