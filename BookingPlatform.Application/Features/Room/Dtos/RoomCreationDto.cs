using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class RoomCreationDto
    {
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }

        public int AdultCapacity { get; set; }

        public int ChildrenCapacity { get; set; }
        public bool IsOperational { get; set; }
    }
}
