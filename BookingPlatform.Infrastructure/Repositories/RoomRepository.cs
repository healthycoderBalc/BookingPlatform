using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(BookingPlatformDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<Room>> GetByHotelIdAsync(int hotelId)
        {
            return await _dbContext.Rooms.Where(r => r.HotelId == hotelId).Include(r => r.RoomImages).ToListAsync();
        }
    }
}
