using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        public Task<ICollection<Hotel>> GetHotelsBySearchAsync(string? hotelName, string? cityName, DateTime? checkIn, DateTime? checkOut, int? adults, int? children);
    }
}
