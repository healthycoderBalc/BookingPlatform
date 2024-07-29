using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Commands.AddToCart
{
    public class AddToCartValidator : AbstractValidator<AddToCartCommand>
    {
        private readonly IRepository<Domain.Entities.Room> _repository;

        public AddToCartValidator(IRepository<Domain.Entities.Room> repository)
        {
            _repository = repository;

            RuleFor(c => c.AddToCartBooking.RoomId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(h => h.AddToCartBooking.CheckInDate)
               .NotEmpty()
               .NotNull()
               .WithMessage("CheckIn date must be provided.");

            RuleFor(h => h.AddToCartBooking.CheckOutDate)
                .NotEmpty()
                .NotNull()
                .WithMessage("CheckOut date must be provided");

            RuleFor(h => h)
                .Must(h => h.AddToCartBooking.CheckOutDate >= h.AddToCartBooking.CheckInDate)
                .WithMessage("CheckIn date must be before or equal to CheckOut date.");

            RuleFor(h => h.AddToCartBooking.RoomId)
               .MustAsync(RoomExist)
               .WithMessage("Room does not exist");
        }

        private async Task<bool> RoomExist(int roomId, CancellationToken cancellation)
        {
            var room = await _repository.GetByIdAsync(roomId).ConfigureAwait(false);
            return room?.Id > 0;
        }

    }
}
