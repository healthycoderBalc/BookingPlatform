using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingPlatformDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Booking> AddToCartAsync(Booking booking, string userId)
        {
            var room = await _dbContext.Rooms.FindAsync(booking.RoomId) ?? throw new Exception("Room not found");
            var totalDays = (booking.CheckOutDate - booking.CheckInDate).Days;
            booking.TotalPrice = totalDays * room.PricePerNight;
            booking.UserId = userId;

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> GetBookingConfirmationAsync(int id)
        {
            var bookingConfirmation = await _dbContext.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(bc => bc.Id == id);
            return bookingConfirmation;
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .Include(b => b.Room)
                    .ThenInclude(b => b.Type)
             .FirstOrDefaultAsync(bc => bc.Id == id);
            return booking;
        }
    }
}
