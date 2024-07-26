using BookingPlatform.Application.Features.City.Dtos;
using MediatR;

namespace BookingPlatform.Application.Features.City.Commands.CreateCity
{
    public class CreateCityCommand : IRequest<CreateCityResponse>
    {
        public CityCreationDto CreateCity {  get; set; }
    }
}
