using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId
{
    public class GetRoomsByHotelIdValidator : AbstractValidator<GetRoomsByHotelIdQuery>
    {
        private readonly IRepository<Domain.Entities.Hotel> _hotelRepository;

        public GetRoomsByHotelIdValidator(IRepository<Domain.Entities.Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;

            RuleFor(c => c.HotelId)
               .GreaterThan(0)
               .WithMessage("{PropertyName} greater than 0");

            RuleFor(h => h.HotelId)
              .MustAsync(HotelExist)
              .WithMessage("Hotel does not exist");
        }

        private async Task<bool> HotelExist(int hotelId, CancellationToken cancellation)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId).ConfigureAwait(false);
            return hotel?.Id > 0;
        }
    }
}
