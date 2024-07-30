using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Interfaces;

namespace BookingPlatform.Domain.Entities
{
    public class HotelReview : ISoftDelete
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int HotelId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ReviewText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
        public Hotel Hotel { get; set; }
    }
}
