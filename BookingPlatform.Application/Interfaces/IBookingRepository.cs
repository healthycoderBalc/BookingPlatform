using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<Booking> AddToCartAsync(Booking booking, string userId);
        public Task<Booking> GetBookingConfirmationAsync(int id);
    }
}
