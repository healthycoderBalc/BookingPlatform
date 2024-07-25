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
    }
}
