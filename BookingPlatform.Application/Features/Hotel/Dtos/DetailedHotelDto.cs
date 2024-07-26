namespace BookingPlatform.Application.Features.Hotel.Dtos
{
    public class DetailedHotelDto
    {
        public string Name { get; set; }
        public int StarRating { get; set; }
        public string LongDescription { get; set; }
        public int CityId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
