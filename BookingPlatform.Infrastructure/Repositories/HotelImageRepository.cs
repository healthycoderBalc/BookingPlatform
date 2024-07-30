using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class HotelImageRepository : Repository<HotelImage>, IHotelImageRepository
    {
        public HotelImageRepository(BookingPlatformDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<HotelImage>> GetByHotelIdAsync(int hotelId)
        {
            return await _dbContext.HotelImages.Where(hi => hi.HotelId == hotelId).ToListAsync();
        }
    }
}
