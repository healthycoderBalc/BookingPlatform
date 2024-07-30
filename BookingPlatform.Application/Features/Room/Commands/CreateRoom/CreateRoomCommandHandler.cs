using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Commands.CreateHotel;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, CreateRoomResponse>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRepository<Domain.Entities.RoomType> _roomTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateRoomCommandHandler(IRepository<Domain.Entities.Room> repository, IHotelRepository hotelRepository, IRepository<Domain.Entities.RoomType> roomTypeRepository, IMapper mapper, ILogger<CreateRoomCommandHandler> logger)
        {
            _repository = repository;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CreateRoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var createRoomResponse = new CreateRoomResponse();
            var validator = new CreateRoomValidator(_hotelRepository, _roomTypeRepository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    createRoomResponse.Success = false;
                    createRoomResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return createRoomResponse;
                }
                else if (createRoomResponse.Success)
                {
                    var roomEntity = _mapper.Map<Domain.Entities.Room>(request.CreateRoom);
                  
                    var result = await _repository.AddAsync(roomEntity);
                    createRoomResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                createRoomResponse.Success = false;
                createRoomResponse.Message = ex.Message;
            }

            return createRoomResponse;
        }
    }
}
