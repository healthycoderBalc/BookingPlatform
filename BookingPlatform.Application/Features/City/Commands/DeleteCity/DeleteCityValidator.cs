using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.DeleteCity
{
    public class DeleteCityValidator : AbstractValidator<DeleteCityCommand>
    {
        private readonly IRepository<Domain.Entities.City> _repository;

        public DeleteCityValidator(IRepository<Domain.Entities.City> repository)
        {
            _repository = repository;

            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(c => c.Id).MustAsync(CityExist).WithMessage("{PropertyName} does not exist.");
        }
        private async Task<bool> CityExist(int cityId, CancellationToken cancellation)
        {
            var city = await _repository.GetByIdAsync(cityId).ConfigureAwait(false);
            return city?.Id > 0;
        }
    }
}
