using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin
{
    public class GetRoomsByFilterAdminQueryHandler : IRequestHandler<GetRoomsByFilterAdminQuery, GetRoomsByFilterAdminResponse>
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetRoomsByFilterAdminQueryHandler(IRoomRepository repository, IMapper mapper, ILogger<GetRoomsByFilterAdminQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetRoomsByFilterAdminResponse> Handle(GetRoomsByFilterAdminQuery request, CancellationToken cancellationToken)
        {
            var roomResponse = new GetRoomsByFilterAdminResponse();
            var validator = new GetRoomsByFilterAdminValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    roomResponse.Success = false;
                    roomResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return roomResponse;
                }
                else if (roomResponse.Success)
                {
                    var result = await _repository.GetRoomsByFilterAdminAsync(request.RoomFilter.RoomNumber, request.RoomFilter.Availability, request.RoomFilter.AdultCapacity, request.RoomFilter.ChildrenCapacity, request.RoomFilter.CreationDate, request.RoomFilter.ModificationDate);
                    roomResponse.FilteredRooms = _mapper.Map<ICollection<RoomAdminDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                roomResponse.Success = false;
                roomResponse.Message = ex.Message;
            }

            return roomResponse;
        }
    }
}
