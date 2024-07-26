using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class RoomAdminDto : BaseDto
    {
        public string RoomNumber { get; set; }
        public bool Availability { get; set; } // is operational
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
