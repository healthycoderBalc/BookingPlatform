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
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(BookingPlatformDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<(City City, int NumberOfVisits)>> GetTrendingCitiesAsync()
        {
            var topCities = await _dbContext.Bookings
               .GroupBy(b => b.Room.Hotel.City)
               .OrderByDescending(g => g.Count())
               .Select(g => new
               {
                   City = g.Key,
                   NumberOfVisits = g.Count()
               })
               .Take(5)
               .ToListAsync();
            var result = topCities.Select(x => (x.City, x.NumberOfVisits)).ToList();

            return result;
        }
    }
}
