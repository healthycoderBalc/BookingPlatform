using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.FeaturedDeal.Dtos
{
    public class FeaturedHotelDto : BaseDto
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StarRating { get; set; }
        public string RoomType { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
