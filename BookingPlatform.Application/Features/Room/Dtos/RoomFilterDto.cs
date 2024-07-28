using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class RoomFilterDto : BaseDto
    {
        public string? RoomNumber { get; set; }
        public bool? Availability { get; set; } // is operational
        public int? AdultCapacity { get; set; }
        public int? ChildrenCapacity { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public RoomFilterDto(string? roomNumber, bool? availability, int? adultCapacity, int? childrenCapacity, DateTime? creationDate, DateTime? modificationDate)
        { 
            RoomNumber = roomNumber;
            Availability = availability;
            AdultCapacity = adultCapacity;
            ChildrenCapacity = childrenCapacity;
            CreationDate = creationDate;
            ModificationDate = modificationDate;

        }
    }
}
