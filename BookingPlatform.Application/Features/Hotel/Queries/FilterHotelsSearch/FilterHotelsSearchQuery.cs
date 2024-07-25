using MediatR;

namespace BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch
{
    public class FilterHotelsSearchQuery : IRequest<FilterHotelsSearchResponse>
    {
        public ICollection<int> HotelIds { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStarRating { get; set; }
        public int? MaxStarRating { get; set; }
        public ICollection<string>? Amenities { get; set; }

        public ICollection<string>? RoomTypes { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
