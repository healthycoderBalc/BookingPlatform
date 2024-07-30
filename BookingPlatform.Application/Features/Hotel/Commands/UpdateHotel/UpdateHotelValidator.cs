using BookingPlatform.Application.Interfaces;
using FluentValidation;

namespace BookingPlatform.Application.Features.Hotel.Commands.UpdateHotel
{
    public class UpdateHotelValidator : AbstractValidator<UpdateHotelCommand>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;
        private readonly IRepository<Domain.Entities.City> _cityRepository;

        public UpdateHotelValidator(IRepository<Domain.Entities.Hotel> repository, IRepository<Domain.Entities.City> cityRepository)
        {
            _repository = repository;
            _cityRepository = cityRepository;


            RuleFor(h => h.UpdateHotel.Name)
             .NotEmpty()
             .NotNull()
             .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.UpdateHotel.Address)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.UpdateHotel.CityId)
              .GreaterThan(0)
              .WithMessage("{PropertyName} should be greater than 0");

            RuleFor(h => h.UpdateHotel.OwnerName)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.UpdateHotel.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90 degrees.");

            RuleFor(h => h.UpdateHotel.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180 degrees.");

            RuleFor(c => c.Id)
                .MustAsync(HotelExist)
                .WithMessage("{PropertyName} does not exist");

            RuleFor(c => c.UpdateHotel.CityId)
              .MustAsync(CityExist)
              .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> HotelExist(int hotelId, CancellationToken cancellation)
        {
            var hotel = await _repository.GetByIdAsync(hotelId).ConfigureAwait(false);
            return hotel?.Id > 0;
        }

        private async Task<bool> CityExist(int cityId, CancellationToken cancellation)
        {
            var city = await _cityRepository.GetByIdAsync(cityId).ConfigureAwait(false);
            return city?.Id > 0;
        }
    }
}
