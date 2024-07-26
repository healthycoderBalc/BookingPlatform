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
        public GetHotelImagesByHotelIdValidator()
        {
            RuleFor(c => c.HotelId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");
        }
    }
}
