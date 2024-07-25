using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        public Task<ICollection<Hotel>> GetHotelsBySearchAsync(string? hotelName, string? cityName, DateTime? checkIn, DateTime? checkOut, int? adults, int? children);
        public Task<ICollection<(Hotel, Room)>> GetRecentHotelsByAuthUserAsync(string userId);

        public Task<(ICollection<(Hotel Hotel, ICollection<decimal> pricesPerNight)> Hotels, int TotalCount, int TotalCountThisPage)> FilterHotelsAsync(
            ICollection<int> searchedHotelIds, 
            decimal? minPrice, 
            decimal? maxPrice, 
            int? minStarRating, 
            int? maxStarRating, 
            ICollection<string>? amenities, 
            ICollection<string>? roomTypes,
            int pageNumber,
            int pageSize
            );

    }
}
