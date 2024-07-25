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
    }
}
