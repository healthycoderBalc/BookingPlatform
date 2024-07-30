using BookingPlatform.Application.Features.Entity;
using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class DetailedRoomDto : BaseDto
    {
        public string RoomNumber { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public ICollection<RoomImageDto> RoomImages { get; set; }
    }
}
