namespace N_HMS.DTO
{
    public class FloorDTO
    {
        public int No { get; set; }
        public int Id { get; set; }
        public string FloorName { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }
    }

    public class FloorSelectDTO
    {
        public int Id { get; set; }
        public string FloorName { get; set; } = null!;
    }
}
