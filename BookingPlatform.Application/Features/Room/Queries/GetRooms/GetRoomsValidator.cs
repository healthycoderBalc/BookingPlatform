using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRooms
{
    public class GetRoomsValidator : AbstractValidator<GetRoomsQuery>
    {
        public GetRoomsValidator()
        {
            
        }
    }
}
