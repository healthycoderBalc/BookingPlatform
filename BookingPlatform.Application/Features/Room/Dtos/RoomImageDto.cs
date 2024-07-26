using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class RoomImageDto : BaseDto
    {
        public string ImageUrl { get; set; }
    }
}
