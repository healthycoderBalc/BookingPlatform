using AutoMapper;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId
{
    public class GetRoomsByHotelIdQueryHandler : IRequestHandler<GetRoomsByHotelIdQuery, GetRoomsByHotelIdResponse>
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetRoomsByHotelIdQueryHandler(IRoomRepository repository, IMapper mapper, ILogger<GetRoomsByHotelIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetRoomsByHotelIdResponse> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelImagesResponse = new GetRoomsByHotelIdResponse();
            var validator = new GetRoomsByHotelIdValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    hotelImagesResponse.Success = false;
                    hotelImagesResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return hotelImagesResponse;
                }
                else if (hotelImagesResponse.Success)
                {
                    var result = await _repository.GetByHotelIdAsync(request.HotelId);
                    hotelImagesResponse.DetailedRooms = _mapper.Map<ICollection<DetailedRoomDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                hotelImagesResponse.Success = false;
                hotelImagesResponse.Message = ex.Message;
            }

            return hotelImagesResponse;
        }
    }
}
