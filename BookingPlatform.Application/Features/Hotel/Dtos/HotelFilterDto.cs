using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class HotelFilterDto : BaseDto
    {
        public string? Name { get; set; }
        public int? StarRating { get; set; }
        public string? OwnerName { get; set; }
        public int? NumberOfRooms { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public HotelFilterDto(string? name, int? starRating, string? ownerName, int? numberOfRooms, DateTime? creationDate, DateTime? modificationDate)
        {
            Name = name;
            StarRating = starRating;
            OwnerName = ownerName;
            NumberOfRooms = numberOfRooms;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }
    }
}
