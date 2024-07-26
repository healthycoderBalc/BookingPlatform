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

        public async Task<ICollection<(City City, int NumberOfHotels)>> GetCitiesAdminAsync()
        {
            var citiesAdmin = await _dbContext.Cities
                 .GroupJoin(
                     _dbContext.Hotels,
                     city => city.Id,
                     hotel => hotel.CityId,
                     (city, hotels) => new
                     {
                         City = city,
                         NumberOfHotels = hotels.Count()
                     })
                 .ToListAsync();

            var result = citiesAdmin.Select(x => (x.City, x.NumberOfHotels)).ToList();

            return result;
        }

        public async Task<ICollection<(City City, int NumberOfHotels)>> GetCitiesByFilterAsync(string? name, string? country, string? postalCode, int? numberOfHotels, DateTime? creationDate, DateTime? modificationDate)
        {
            var query = _dbContext.Cities
                 .GroupJoin(
                     _dbContext.Hotels,
                     city => city.Id,
                     hotel => hotel.CityId,
                     (city, hotels) => new
                     {
                         City = city,
                         NumberOfHotels = hotels.Count()
                     })
                 .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(h => h.City.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(h => h.City.Country.Contains(country));
            }


            if (!string.IsNullOrEmpty(postalCode))
            {
                query = query.Where(h => h.City.PostalCode.Contains(postalCode));
            }

            if (numberOfHotels.HasValue)
            {
                query = query.Where(h => h.NumberOfHotels == numberOfHotels.Value);
            }

            if (creationDate.HasValue)
            {
                query = query.Where(h => h.City.CreatedAt.Date == creationDate.Value.Date);
            }

            if (modificationDate.HasValue)
            {
                query = query.Where(h => h.City.UpdatedAt.Date == modificationDate.Value.Date);
            }

            var resultList = await query.ToListAsync();
            var result = resultList.Select(x => (x.City, x.NumberOfHotels)).ToList();

            return result;
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
