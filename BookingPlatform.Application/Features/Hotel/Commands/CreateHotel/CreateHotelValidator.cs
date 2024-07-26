using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelValidator : AbstractValidator<CreateHotelCommand>
    {
        private readonly IRepository<Domain.Entities.City> _cityRepository;
        public CreateHotelValidator(IRepository<Domain.Entities.City> cityRepository)
        {
            _cityRepository = cityRepository;

            RuleFor(h => h.CreateHotel.Name)
             .NotEmpty()
             .NotNull()
             .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateHotel.ThumbnailUrl)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateHotel.ShortDescription)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateHotel.LongDescription)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateHotel.Address)
              .NotEmpty()
              .NotNull()
              .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateHotel.CityId)
              .GreaterThan(0)
              .WithMessage("{PropertyName} should be greater than 0");

            RuleFor(h => h.CreateHotel.StarRating)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(5)
                .WithMessage("{PropertyName} should have a value between 1 and 5");

            RuleFor(h => h.CreateHotel.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90 degrees.");

            RuleFor(h => h.CreateHotel.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180 degrees.");

            RuleFor(h => h.CreateHotel.CityId)
              .MustAsync(CityExist)
              .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> CityExist(int cityId, CancellationToken cancellation)
        {
            var city = await _cityRepository.GetByIdAsync(cityId).ConfigureAwait(false);
            return city?.Id > 0;
        }
    }
}
