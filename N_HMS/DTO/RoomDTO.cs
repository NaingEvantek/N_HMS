using N_HMS.Models;

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

    public class RoomWithBookingDto
    {
        public Room_Info Room { get; set; } = new();
        public Booking_Info? ActiveBooking { get; set; } = new();
    }


    public class RoomTypeSelectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class RoomCustomDTO
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public decimal? PricePerDay { get; set; }
        public int? RoomCapacityAdult { get; set; }
        public int? RoomCapacityChild { get; set; }

        // Readable names from navigation properties
        public string FloorName { get; set; } = string.Empty;
        public string RoomTypeName { get; set; } = string.Empty;
        public string RoomStatusName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
    }
    public enum RoomStatusEnum
    {
        Available = 1,
        Occupied = 2,
        Cleaning = 3
    }

    public enum PaymentStatusEnum
    {
        Paid =1,
        Unpaid=2
    }
}
