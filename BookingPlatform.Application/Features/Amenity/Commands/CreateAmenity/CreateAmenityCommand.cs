using BookingPlatform.Application.Features.Amenity.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Amenity.Commands.CreateAmenity
{
    public class CreateAmenityCommand : IRequest<CreateAmenityResponse>
    {
        public AmenityDto CreateAmenity { get; set; }
    }
}
