using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Commands.UpdateHotel;
using BookingPlatform.Application.Features.Room.Commands.CreateRoom;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, UpdateRoomResponse>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;
        private readonly IRepository<Domain.Entities.Hotel> _hotelRepository;
        private readonly IRepository<Domain.Entities.RoomType> _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateRoomCommandHandler(IRepository<Domain.Entities.Room> repository, IRepository<Domain.Entities.Hotel> hotelRepository, IRepository<Domain.Entities.RoomType> roomTypeRepository, IMapper mapper, ILogger<UpdateRoomCommandHandler> logger)
        {
            _repository = repository;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UpdateRoomResponse> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var updateRoomResponse = new UpdateRoomResponse();
            var validator = new UpdateRoomValidator(_repository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    updateRoomResponse.Success = false;
                    updateRoomResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return updateRoomResponse;
                }
                else if (updateRoomResponse.Success)
                {
                    var roomEntity = await _repository.GetByIdAsync(request.Id);
                    _mapper.Map(request.RoomUpdate, roomEntity);

                  
                    await _repository.UpdateAsync(roomEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                updateRoomResponse.Success = false;
                updateRoomResponse.Message = ex.Message;
            }

            return updateRoomResponse;
        }
    }
}
