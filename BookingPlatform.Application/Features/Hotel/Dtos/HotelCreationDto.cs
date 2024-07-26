using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class HotelCreationDto
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StarRating { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string OwnerName { get; set; }
    }
}
