using BookingPlatform.Application.Features.City.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.City.Commands.UpdateCity
{
    public class UpdateCityCommand : BaseCommandQuery, IRequest<UpdateCityResponse>
    {
        public CityUpdateDto UpdateCity {  get; set; }
    }
}
