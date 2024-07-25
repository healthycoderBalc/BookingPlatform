using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class FilterHotelDto : BaseDto
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public int StarRating { get; set; }
        public ICollection<decimal> PricesPerNight { get; set; }
    }
}
