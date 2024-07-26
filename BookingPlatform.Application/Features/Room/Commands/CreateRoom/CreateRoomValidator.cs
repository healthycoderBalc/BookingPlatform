using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
    {
        private readonly IRepository<Domain.Entities.Hotel> _hotelRepository;
        private readonly IRepository<Domain.Entities.RoomType> _roomTypeRepository;

        public CreateRoomValidator(IRepository<Domain.Entities.Hotel> hotelRepository, IRepository<Domain.Entities.RoomType> roomTypeRepository)
        {
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;

            RuleFor(h => h.CreateRoom.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateRoom.RoomNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateRoom.HotelId)
              .GreaterThan(0)
              .WithMessage("{PropertyName} should be greater than 0");

            RuleFor(h => h.CreateRoom.TypeId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} should be greater than 0");

            RuleFor(r => r.CreateRoom.AdultCapacity)
              .GreaterThanOrEqualTo(1)
              .WithMessage("Number of adults must be at least 1.");

            RuleFor(r => r.CreateRoom.ChildrenCapacity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of children cannot be negative.");

            RuleFor(h => h.CreateRoom.PricePerNight)
                 .NotEmpty()
                 .NotNull()
                 .WithMessage("{PropertyName} should have a value");

            RuleFor(h => h.CreateRoom.IsOperational)
               .NotEmpty()
               .NotNull()
               .WithMessage("{PropertyName} should have a value");

            RuleFor(c => c.CreateRoom.HotelId)
             .MustAsync(HotelExist)
             .WithMessage("{PropertyName} does not exist");

            RuleFor(c => c.CreateRoom.TypeId)
            .MustAsync(RoomTypeExist)
            .WithMessage("{PropertyName} does not exist");

        }

        private async Task<bool> HotelExist(int hotelId, CancellationToken cancellation)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId).ConfigureAwait(false);
            return hotel?.Id > 0;
        }

        private async Task<bool> RoomTypeExist(int roomTypeId, CancellationToken cancellation)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId).ConfigureAwait(false);
            return roomType?.Id > 0;
        }
    }
}
