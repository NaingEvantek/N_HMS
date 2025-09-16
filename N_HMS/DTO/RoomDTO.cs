namespace N_HMS.DTO
{
    public class RoomDTO
    {
        public int No { get; set; }
        public int Id { get; set; }
        public string RoomName { get; set; } = null!;
        public string? FloorName { get; set; }
        public int? FloorId { get; set; }
        public string? RoomType { get; set; }
        public int? RoomTypeId { get; set; }
        public string? RoomStatus { get; set; }
        public int? RoomStatusId { get; set; }
        public decimal? PricePerDay { get; set; }
        public string? CurrencyType { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string? CurrencyCode { get; set; }
        public int? RoomCapacityAdult { get; set; }
        public int? RoomCapacityChild { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
