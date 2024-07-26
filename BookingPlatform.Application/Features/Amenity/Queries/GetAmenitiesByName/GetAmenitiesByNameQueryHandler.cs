using AutoMapper;
using BookingPlatform.Application.Features.Amenity.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Amenity.Queries.GetAmenitiesByName
{
    public class GetAmenitiesByNameQueryHandler : IRequestHandler<GetHotelImagesByhotelIdQuery, GetAmenitiesByNameResponse>
    {
        private readonly IAmenityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAmenitiesByNameQueryHandler> _logger;

        public GetAmenitiesByNameQueryHandler(IAmenityRepository repository, IMapper mapper, ILogger<GetAmenitiesByNameQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetAmenitiesByNameResponse> Handle(GetHotelImagesByhotelIdQuery request, CancellationToken cancellationToken)
        {
            var amenityResponse = new GetAmenitiesByNameResponse();
            var validator = new GetAmenitiesByNameValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    amenityResponse.Success = false;
                    amenityResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        amenityResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                }
                else if (amenityResponse.Success)
                {
                    var result = await _repository.GetByNameAsync(request.Name);
                    amenityResponse.Amenities = _mapper.Map<ICollection<AmenityDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                amenityResponse.Success = false;
                amenityResponse.Message = ex.Message;
            }

            return amenityResponse;
        }
    }
}
