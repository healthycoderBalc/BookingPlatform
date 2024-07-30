using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Interfaces;

namespace BookingPlatform.Domain.Entities
{
    public class HotelAmenity : ISoftDelete
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int AmenityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Hotel Hotel { get; set; }
        public Amenity Amenity { get; set; }
    }
}
