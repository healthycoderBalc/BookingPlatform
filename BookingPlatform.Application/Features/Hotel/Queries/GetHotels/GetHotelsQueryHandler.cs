using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Hotel.Queries.GetHotels
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, GetHotelsResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHotelsQueryHandler> _logger;

        public GetHotelsQueryHandler(IHotelRepository repository, IMapper mapper, ILogger<GetHotelsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetHotelsResponse> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            var getHotelResponse = new GetHotelsResponse();
            var validator = new GetHotelsValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    getHotelResponse.Success = false;
                    getHotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return getHotelResponse;
                }
                else if (getHotelResponse.Success)
                {
                    var result = await _repository.GetHotelsAdminAsync();
                    getHotelResponse.HotelsAdmin = _mapper.Map<ICollection<HotelAdminDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getHotelResponse.Success = false;
                getHotelResponse.Message = ex.Message;
            }

            return getHotelResponse;
        }
    }
}
