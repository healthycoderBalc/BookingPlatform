using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Interfaces;

namespace BookingPlatform.Domain.Entities
{
    public class Amenity : ISoftDelete
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public ICollection<HotelAmenity> HotelAmenities { get; set; }
    }
}
