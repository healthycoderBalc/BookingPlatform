using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Commands.CreateHotel;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Hotel.Commands.UpdateHotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, UpdateHotelResponse>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;
        private readonly IRepository<Domain.Entities.City> _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateHotelCommandHandler(IRepository<Domain.Entities.Hotel> repository, IRepository<Domain.Entities.City> cityRepository , IMapper mapper, ILogger<UpdateHotelCommandHandler> logger)
        {
            _repository = repository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UpdateHotelResponse> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var updateHotelResponse = new UpdateHotelResponse();
            var validator = new UpdateHotelValidator(_repository, _cityRepository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    updateHotelResponse.Success = false;
                    updateHotelResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return updateHotelResponse;
                }
                else if (updateHotelResponse.Success)
                {
                    var hotelEntity = await _repository.GetByIdAsync(request.Id);
                    _mapper.Map(request.UpdateHotel, hotelEntity);
                  
                    await _repository.UpdateAsync(hotelEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                updateHotelResponse.Success = false;
                updateHotelResponse.Message = ex.Message;
            }

            return updateHotelResponse;
        }
    }
}
