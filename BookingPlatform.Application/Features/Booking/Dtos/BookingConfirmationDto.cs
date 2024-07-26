using BookingPlatform.Application.Features.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Dtos
{
    public class BookingConfirmationDto : BaseDto
    {
        public string ConfirmationNumber { get; set; } = string.Empty;
        public string HotelAddress { get; set; }
        public string RoomNumber { get; set; }
        public string RoomDescription { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public decimal TotalPrice { get; set; }
        public string SpecialRequests { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
