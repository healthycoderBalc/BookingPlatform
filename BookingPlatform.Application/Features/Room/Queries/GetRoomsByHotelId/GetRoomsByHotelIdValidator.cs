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
        public GetRoomsByHotelIdValidator()
        {
            RuleFor(c => c.HotelId)
               .GreaterThan(0)
               .WithMessage("{PropertyName} greater than 0");
        }
    }
}
