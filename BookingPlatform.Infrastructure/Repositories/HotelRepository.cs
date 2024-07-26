using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {

        public HotelRepository(BookingPlatformDbContext dbContext) : base(dbContext) { }


        public async Task<ICollection<Hotel>> GetHotelsBySearchAsync(string? hotelName, string? cityName, DateTime? checkIn, DateTime? checkOut, int? adults, int? children)
        {
            var query = _dbContext.Hotels
                .Include(h => h.City)
                .Include(h => h.Rooms)
                .AsQueryable();

            if (!string.IsNullOrEmpty(hotelName))
            {
                query = query.Where(h => h.Name.Contains(hotelName));
            }

            if (!string.IsNullOrEmpty(cityName))
            {
                query = query.Where(h => h.City.Name.Contains(cityName));
            }

            if (checkIn.HasValue)
            {
                query = query.Where(
                    h => h.Rooms.Any(r => !r.Bookings
                        .Any(b => b.CheckInDate <= checkOut && b.CheckOutDate >= checkIn)
                        )
                    );
            }
            else
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = DateTime.Today.AddDays(1);
                query = query.Where(
                  h => h.Rooms.Any(r => !r.Bookings
                      .Any(b => b.CheckInDate <= tomorrow && b.CheckOutDate >= today)
                      )
                  );
            }

            if (adults.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.AdultCapacity >= adults));
            }
            else
            {
                query = query.Where(h => h.Rooms.Any(r => r.AdultCapacity >= 2));
            }

            if (children.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.ChildrenCapacity >= children));
            }
            else
            {
                query = query.Where(h => h.Rooms.Any(r => r.ChildrenCapacity >= 0));
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<(Hotel, Room)>> GetRecentHotelsByAuthUserAsync(string userId)
        {
            var recentHotelRooms = await _dbContext.Bookings
                 .Where(b => b.UserId == userId)
                 .OrderByDescending(b => b.CheckOutDate)
                 .Take(5)
                 .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                        .ThenInclude(h => h.City)
                 .Select(b => new { b.Room.Hotel, b.Room })
                 .ToListAsync();

            var result = recentHotelRooms.Select(hr => (hr.Hotel, hr.Room)).Distinct().ToList();

            return result;
        }
        public async Task<(ICollection<(Hotel Hotel, ICollection<decimal> pricesPerNight)>Hotels, int TotalCount, int TotalCountThisPage)> FilterHotelsAsync(
            ICollection<int> searchedHotelIds, 
            decimal? minPrice, 
            decimal? maxPrice, 
            int? minStarRating, 
            int? maxStarRating, 
            ICollection<string>? amenities, 
            ICollection<string>? roomTypes,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _dbContext.Hotels
                .AsQueryable();
            query = query
                .Where(h => searchedHotelIds.Contains(h.Id));

            if (minPrice.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.PricePerNight >= minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.PricePerNight <= maxPrice.Value));
            }

            if (minStarRating.HasValue)
            {
                query = query.Where(h => h.StarRating >= minStarRating.Value);
            }

            if (maxStarRating.HasValue)
            {
                query = query.Where(h => h.StarRating <= maxStarRating.Value);
            }

            if (amenities != null && amenities.Count != 0)
            {
                query = query.Where(h => h.HotelAmenities.Any(a => amenities.Contains(a.Amenity.Name)));
            }

            if (roomTypes != null && roomTypes.Count != 0)
            {
                query = query.Where(h => h.Rooms.Any(r => roomTypes.Contains(r.Type.Name)));
            }

            var totalCount = await query.CountAsync();

            var pagedQuery = query
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize);

            var resultQuery = await pagedQuery
                  .Select(h => new
                  {
                      Hotel = h,
                      PricesPerNight = h.Rooms
                        .Select(r => r.PricePerNight)
                        .ToList()
                  })
                .ToListAsync();
            var includingPrices = resultQuery
                .Select(x => (x.Hotel, (ICollection<decimal>)x.PricesPerNight)).ToList();

            return (includingPrices, totalCount, includingPrices.Count);
        }

        public new async Task<Hotel> GetByIdAsync(int id)
        {
            var hotel = await _dbContext.Hotels
                 .Include(h => h.HotelReviews)
                 .FirstOrDefaultAsync(h => h.Id == id);
            return hotel;
        }
    }
}
