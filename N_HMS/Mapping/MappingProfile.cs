using AutoMapper;

namespace N_HMS.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Room_Info,DTO.RoomCustomDTO>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room_Name))
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.Price_Per_Day))
                .ForMember(dest => dest.RoomCapacityAdult, opt => opt.MapFrom(src => src.Room_Capacity_Adult))
                .ForMember(dest => dest.RoomCapacityChild, opt => opt.MapFrom(src => src.Room_Capacity_Child))

                .ForMember(dest => dest.FloorName, opt => opt.MapFrom(src => src.Floor.Name))
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.Room_Type.Name))
                .ForMember(dest => dest.RoomStatusName, opt => opt.MapFrom(src => src.Room_Status.Status))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency_Type.Code));
           
        }
    }
}
