using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId
{
    public class GetHotelImagesByHotelIdValidator : AbstractValidator<GetHotelImagesByHotelIdQuery>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;

        public GetHotelImagesByHotelIdValidator(IRepository<Domain.Entities.Hotel> repository)
        {
            _repository = repository;

            RuleFor(c => c.HotelId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(h => h.HotelId)
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
