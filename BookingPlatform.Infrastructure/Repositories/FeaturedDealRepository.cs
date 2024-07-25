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
    public class FeaturedDealRepository : Repository<FeaturedDeal>, IFeaturedDealRepository
    {
        public FeaturedDealRepository(BookingPlatformDbContext dbContext) : base(dbContext) { }
        public async Task<ICollection<FeaturedDeal>> GetFeaturedDealsAsync()
        {
            return await _dbContext.FeaturedDeals
                .Where(fd => fd.DealActive)
                .OrderByDescending(fd => fd.CreatedAt)
                .Include(fd => fd.Room)
                    .ThenInclude(room => room.Hotel)
                        .ThenInclude(hotel => hotel.City)
                .Include(fd => fd.Room)
                    .ThenInclude(room => room.Type)
                .Take(5)
                .ToListAsync();
        }
    }
}
