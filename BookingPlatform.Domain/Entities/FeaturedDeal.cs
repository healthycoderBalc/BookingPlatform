using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Domain.Entities
{
    public class FeaturedDeal
    {
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }
        public bool DealActive { get; set; }

        [Required]
        public decimal DiscountedPricePerNight { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Room Room { get; set; }
    }
}
