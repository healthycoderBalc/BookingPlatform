using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public string RoomNumber { get; set; }
        [Required]
        public int TypeId { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }

        [Required]
        public int AdultCapacity { get; set; }

        [Required]
        public int ChildrenCapacity { get; set; }
        public bool IsOperational { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Hotel Hotel { get; set; }
        public RoomType Type { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; }
        public ICollection<FeaturedDeal> FeaturedDeals { get; set; }
    }
}
