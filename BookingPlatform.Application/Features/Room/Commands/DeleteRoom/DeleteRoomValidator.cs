using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Commands.DeleteRoom
{
    public class DeleteRoomValidator : AbstractValidator<DeleteRoomCommand>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;
        public DeleteRoomValidator(IRepository<Domain.Entities.Room> repository)
        {
            _repository = repository;

            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(c => c.Id).MustAsync(RoomExist).WithMessage("{PropertyName} does not exist.");
            
        }

        private async Task<bool> RoomExist(int roomId, CancellationToken cancellation)
        {
            var room = await _repository.GetByIdAsync(roomId).ConfigureAwait(false);
            return room?.Id > 0;
        }
    }
}
