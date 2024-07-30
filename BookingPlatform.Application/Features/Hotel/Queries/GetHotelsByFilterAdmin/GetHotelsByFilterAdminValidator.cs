using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin
{
    public class GetHotelsByFilterAdminValidator : AbstractValidator<GetHotelsByFilterAdminQuery>
    {
        public GetHotelsByFilterAdminValidator()
        {

            RuleFor(h => h.HotelFilter.StarRating)
                .GreaterThanOrEqualTo(1)
                .WithMessage("{PropertyName} should have a value between 1 and 5")
                .LessThanOrEqualTo(5)
                .WithMessage("{PropertyName} should have a value between 1 and 5");

            RuleFor(x => x.HotelFilter.NumberOfRooms)
             .GreaterThanOrEqualTo(0)
             .WithMessage("Number of hotels must be a non-negative number.");

            RuleFor(x => x.HotelFilter.CreationDate)
               .LessThanOrEqualTo(DateTime.Now)
               .WithMessage("Creation date must be in the past.")
               .When(x => x.HotelFilter.CreationDate.HasValue);

            RuleFor(x => x.HotelFilter.ModificationDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Modification date must be in the past.")
                .When(x => x.HotelFilter.ModificationDate.HasValue);
        }
    }
}
