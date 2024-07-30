using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Interfaces;

namespace BookingPlatform.Domain.Entities
{
    public class City : ISoftDelete
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public ICollection<Hotel> Hotels { get; set; }
    }
}
