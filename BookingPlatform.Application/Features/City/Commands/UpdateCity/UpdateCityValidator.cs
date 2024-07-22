using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.UpdateCity
{
    public class UpdateCityValidator : AbstractValidator<UpdateCityCommand>
    {
        private readonly IRepository<Domain.Entities.City> _repository;

        public UpdateCityValidator(IRepository<Domain.Entities.City> repository)
        {
            _repository = repository;

            RuleFor(c => c.UpdateCity.Name)
             .NotEmpty()
             .NotNull()
             .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.UpdateCity.Country)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.Id)
                .MustAsync(cityExist)
                .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> cityExist(int cityId, CancellationToken cancellation)
        {
            var city = await _repository.GetByIdAsync(cityId).ConfigureAwait(false);
            return city?.Id > 0;
        }
    }
}
