using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch
{
    public class GetHotelsBySearchQuery : IRequest<GetHotelsBySearchResponse>
    {
        public string? HotelName { get; set; }
        public string? CityName { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Adults { get; set; }
        public int? Children {  get; set; }
    }
}
