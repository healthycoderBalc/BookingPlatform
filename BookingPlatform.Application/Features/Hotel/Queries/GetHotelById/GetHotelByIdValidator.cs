using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdValidator : AbstractValidator<GetHotelByIdQuery>
    {
        private readonly IHotelRepository _repository;

        public GetHotelByIdValidator(IHotelRepository repository)
        {
            _repository = repository;

            RuleFor(c => c.Id)
               .GreaterThan(0)
               .WithMessage("{PropertyName} greater than 0");

            RuleFor(h => h.Id)
              .MustAsync(HotelExist)
              .WithMessage("Hotel does not exist");
        }

        private async Task<bool> HotelExist(int hotelId, CancellationToken cancellation)
        {
            var hotel = await _repository.GetByIdAsync(hotelId).ConfigureAwait(false);
            return hotel?.Id > 0;
        }
    }
}
