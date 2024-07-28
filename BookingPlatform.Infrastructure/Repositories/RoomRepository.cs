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

        public async Task<ICollection<Room>> GetRoomsByFilterAdminAsync(string? roomNumber, bool? availability, int? adultCapacity, int? childrenCapacity, DateTime? creationDate, DateTime? modificationDate)
        {
            var query = _dbContext.Rooms
               .AsQueryable();

            if (!string.IsNullOrEmpty(roomNumber))
            {
                query = query.Where(r => r.RoomNumber.Contains(roomNumber));
            }


            if (availability.HasValue)
            {
                query = query.Where(r => r.IsOperational == availability);
            }

            if (adultCapacity.HasValue)
            {
                query = query.Where(r => r.AdultCapacity == adultCapacity.Value);
            }

            if (childrenCapacity.HasValue)
            {
                query = query.Where(r => r.ChildrenCapacity == childrenCapacity.Value);
            }

            if (creationDate.HasValue)
            {
                query = query.Where(r => r.Hotel.CreatedAt.Date == creationDate.Value.Date);
            }

            if (modificationDate.HasValue)
            {
                query = query.Where(r => r.Hotel.UpdatedAt.Date == modificationDate.Value.Date);
            }

            var result = await query.ToListAsync();

            return result;
        }
    }
}
