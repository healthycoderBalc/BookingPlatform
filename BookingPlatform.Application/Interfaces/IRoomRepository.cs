using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<ICollection<Room>> GetByHotelIdAsync(int hotelId);

        public Task<ICollection<Room>> GetRoomsByFilterAdminAsync(string? roomNumber, bool? availability, int? adultCapacity, int? childrenCapacity, DateTime? creationDate, DateTime? modificationDate);
    }
}
