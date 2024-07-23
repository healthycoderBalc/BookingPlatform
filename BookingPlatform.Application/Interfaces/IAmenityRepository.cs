using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        Task<ICollection<Amenity>> GetByNameAsync(string name);
    }
}
