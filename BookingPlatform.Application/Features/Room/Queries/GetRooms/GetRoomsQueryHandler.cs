using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Room.Queries.GetRooms
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, GetRoomsResponse>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoomsQueryHandler> _logger;

        public GetRoomsQueryHandler(IRepository<Domain.Entities.Room> repository, IMapper mapper, ILogger<GetRoomsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetRoomsResponse> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var getRoomResponse = new GetRoomsResponse();
            var validator = new GetRoomsValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    getRoomResponse.Success = false;
                    getRoomResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return getRoomResponse;
                }
                else if (getRoomResponse.Success)
                {
                    var result = await _repository.GetAllAsync();
                    getRoomResponse.RoomsAdmin = _mapper.Map<ICollection<RoomAdminDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                getRoomResponse.Success = false;
                getRoomResponse.Message = ex.Message;
            }

            return getRoomResponse;
        }
    }
}
