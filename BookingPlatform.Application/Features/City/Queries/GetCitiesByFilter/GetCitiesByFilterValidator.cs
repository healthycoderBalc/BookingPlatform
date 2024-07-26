using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter
{
    public class GetCitiesByFilterValidator :AbstractValidator<GetCitiesByFilterQuery>
    {
        public GetCitiesByFilterValidator()
        {
            RuleFor(x => x.CityFilter.Name)
                .MaximumLength(100)
                .WithMessage("City name must not exceed 100 characters.");

            RuleFor(x => x.CityFilter.Country)
                .MaximumLength(100)
                .WithMessage("Country name must not exceed 100 characters.");

            RuleFor(x => x.CityFilter.PostalCode)
                .MaximumLength(20)
                .WithMessage("Postal code must not exceed 20 characters.");

            RuleFor(x => x.CityFilter.NumberOfHotels)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of hotels must be a non-negative number.");

            RuleFor(x => x.CityFilter.CreationDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Creation date must be in the past.")
                .When(x => x.CityFilter.CreationDate.HasValue);

            RuleFor(x => x.CityFilter.ModificationDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Modification date must be in the past.")
                .When(x => x.CityFilter.ModificationDate.HasValue);
        }
    }
}
