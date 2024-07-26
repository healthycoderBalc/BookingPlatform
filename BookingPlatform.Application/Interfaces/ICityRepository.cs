using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        Task<ICollection<(City City, int NumberOfVisits)>> GetTrendingCitiesAsync();

        Task<ICollection<(City City, int NumberOfHotels)>> GetCitiesAdminAsync();

        public Task<ICollection<(City City, int NumberOfHotels)>> GetCitiesByFilterAsync(string? name, string? country, string? postalCode, int? numberOfHotels, DateTime? creationDate, DateTime? modificationDate);
    }
}
