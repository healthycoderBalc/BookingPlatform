using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin
{
    public class GetHotelsByFilterAdminResponse : BaseResponse
    {
        public ICollection<HotelAdminDto> FilteredHotels { get; set; }
    }
}
