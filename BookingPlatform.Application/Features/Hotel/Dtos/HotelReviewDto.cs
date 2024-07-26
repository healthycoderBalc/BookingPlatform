using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class HotelReviewDto : BaseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string ReviewText { get; set; } = string.Empty;
    }
}
