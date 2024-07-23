using AutoMapper;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Amenity.Commands.CreateAmenity
{
    public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, CreateAmenityResponse>
    {
        private readonly IAmenityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAmenityCommandHandler> _logger;
        public CreateAmenityCommandHandler(IAmenityRepository repository, IMapper mapper, ILogger<CreateAmenityCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CreateAmenityResponse> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
        {
            var amenityResponse = new CreateAmenityResponse();
            var validator = new CreateAmenityValidator();
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
                    var amenityEntity = _mapper.Map<Domain.Entities.Amenity>(request.CreateAmenity);
                    var result = await _repository.AddAsync(amenityEntity);
                    amenityResponse.Id = result.Id;
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
