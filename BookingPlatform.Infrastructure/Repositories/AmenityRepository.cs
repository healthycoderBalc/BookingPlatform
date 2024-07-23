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
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {

        public AmenityRepository(BookingPlatformDbContext dbContext) : base(dbContext) { }

        public async Task<ICollection<Amenity>> GetByNameAsync(string name)
        {
            return await _dbContext.Amenities.Where(a => a.Name.Contains(name)).ToListAsync();

        }
    }
}
