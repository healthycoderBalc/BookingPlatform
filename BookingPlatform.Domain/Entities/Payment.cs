using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Booking Booking { get; set; }
    }
}
