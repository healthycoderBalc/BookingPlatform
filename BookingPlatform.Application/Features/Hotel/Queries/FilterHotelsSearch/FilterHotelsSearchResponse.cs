using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch
{
    public class FilterHotelsSearchResponse : BaseResponse
    {
        public ICollection<FilterHotelDto> FilteredHotels { get; set; }
        public int TotalCount { get; set; }
        public int TotalCountThisPage { get; set; }
    }
}
