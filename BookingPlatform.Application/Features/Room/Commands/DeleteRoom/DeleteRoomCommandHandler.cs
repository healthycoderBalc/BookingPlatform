using AutoMapper;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingPlatform.Application.Features.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, DeleteRoomResponse>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteRoomCommandHandler(IRepository<Domain.Entities.Room> repository, IMapper mapper, ILogger<DeleteRoomCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<DeleteRoomResponse> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var deleteRoomResponse = new DeleteRoomResponse();
            var validator = new DeleteRoomValidator(_repository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    deleteRoomResponse.Success = false;
                    deleteRoomResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return deleteRoomResponse;
                }
                else if (deleteRoomResponse.Success)
                {
                    var roomEntity = await _repository.GetByIdAsync(request.Id);
                    await _repository.DeleteAsync(roomEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                deleteRoomResponse.Success = false;
                deleteRoomResponse.Message = ex.Message;
            }

            return deleteRoomResponse;
        }
    }
}
