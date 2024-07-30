using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.HotelImages.Dtos
{
    public class HotelImageDto : BaseDto
    {
        public int HotelId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
