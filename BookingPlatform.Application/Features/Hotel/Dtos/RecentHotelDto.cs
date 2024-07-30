using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class RecentHotelDto : BaseDto
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string CityName { get; set; }
        public int StarRating { get; set; }
        public decimal PricePerNight { get; set; }

    }
}
