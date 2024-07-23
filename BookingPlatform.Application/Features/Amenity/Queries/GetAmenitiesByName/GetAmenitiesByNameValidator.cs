using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Amenity.Queries.GetAmenitiesByName
{
    public class GetAmenitiesByNameValidator : AbstractValidator<GetAmenitiesByNameQuery>
    {
        public GetAmenitiesByNameValidator()
        {
            
        }
    }
}
