using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class HotelAdminDto : BaseDto
    {
        public string Name { get; set; }
        public int StarRating { get; set; }
        public string OwnerName { get; set; }
        public int NumberOfRooms { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }
}
