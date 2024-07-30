using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Commands.UpdateRoom
{
    public class UpdateRoomValidator : AbstractValidator<UpdateRoomCommand>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;

        public UpdateRoomValidator(IRepository<Domain.Entities.Room> repository)
        {
            _repository = repository;


            RuleFor(h => h.RoomUpdate.RoomNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have a value");

            RuleFor(r => r.RoomUpdate.AdultCapacity)
               .GreaterThanOrEqualTo(1)
               .WithMessage("Number of adults must be at least 1.");

            RuleFor(r => r.RoomUpdate.ChildrenCapacity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of children cannot be negative.");

            RuleFor(h => h.RoomUpdate.IsOperational)
               .NotEmpty()
               .NotNull()
               .WithMessage("{PropertyName} should have a value");
            RuleFor(c => c.Id)
                .MustAsync(RoomExist)
                .WithMessage("{PropertyName} does not exist");
        }

        private async Task<bool> RoomExist(int roomId, CancellationToken cancellation)
        {
            var room = await _repository.GetByIdAsync(roomId).ConfigureAwait(false);
            return room?.Id > 0;
        }

    }
}
