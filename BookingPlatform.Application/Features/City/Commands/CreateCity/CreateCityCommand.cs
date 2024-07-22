using BookingPlatform.Application.Features.City.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.CreateCity
{
    public class CreateCityCommand : IRequest<CreateCityResponse>
    {
        public CityDto CreateCity {  get; set; }
    }
}
