namespace BookingPlatform.Application.Features.Booking.Dtos
{
    public class AddToCartBookingDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string SpecialRequests { get; set; }

        public int NumberOfAdults { get; set; }
        public int NumberOfChildren {  get; set; }
    }
}
