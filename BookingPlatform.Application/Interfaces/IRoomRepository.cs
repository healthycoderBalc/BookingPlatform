using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<ICollection<Room>> GetByHotelIdAsync(int hotelId);
    }
}
