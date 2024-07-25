using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Features.FeaturedHotel.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels
{
    public class GetFeaturedHotelsQueryHandler : IRequestHandler<GetFeaturedHotelsQuery, GetFeaturedHotelsResponse>
    {
        private readonly IFeaturedDealRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFeaturedHotelsQueryHandler> _logger;

        public GetFeaturedHotelsQueryHandler(IFeaturedDealRepository repository, IMapper mapper, ILogger<GetFeaturedHotelsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetFeaturedHotelsResponse> Handle(GetFeaturedHotelsQuery request, CancellationToken cancellationToken)
        {
            var featuredHotelResponse = new GetFeaturedHotelsResponse();
            var validator = new GetFeaturedHotelsValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    featuredHotelResponse.Success = false;
                    featuredHotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        featuredHotelResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return featuredHotelResponse;
                }
                else if (featuredHotelResponse.Success)
                {
                    var result = await _repository.GetFeaturedDealsAsync();
                    featuredHotelResponse.FeaturedHotels = _mapper.Map<ICollection<FeaturedHotelDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                featuredHotelResponse.Success = false;
                featuredHotelResponse.Message = ex.Message;
            }

            return featuredHotelResponse;
        }
    }
}
