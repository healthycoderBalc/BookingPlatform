using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingPlatform.Domain.Interfaces;

namespace BookingPlatform.Domain.Entities
{
    public class Hotel : ISoftDelete
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        [Range(1, 5)]
        public int StarRating { get; set; }

        [Range(-90.0, 90.0)]
        public decimal Latitude { get; set; }

        [Range(-180.0, 180.0)]
        public decimal Longitude { get; set; }
        public string OwnerName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public City City { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<HotelReview> HotelReviews { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; }
        public ICollection<HotelAmenity> HotelAmenities { get; set; }
    }
}
