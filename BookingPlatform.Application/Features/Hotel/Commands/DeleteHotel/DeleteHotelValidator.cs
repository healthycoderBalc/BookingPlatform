using BookingPlatform.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Commands.DeleteHotel
{
    public class DeleteHotelValidator : AbstractValidator<DeleteHotelCommand>
    {
        private readonly IRepository<Domain.Entities.Hotel> _repository;
        public DeleteHotelValidator(IRepository<Domain.Entities.Hotel> repository)
        {
            _repository = repository;

            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} greater than 0");

            RuleFor(c => c.Id).MustAsync(HotelExist).WithMessage("{PropertyName} does not exist.");
            
        }
        private async Task<bool> HotelExist(int hotelId, CancellationToken cancellation)
        {
            var hotel = await _repository.GetByIdAsync(hotelId).ConfigureAwait(false);
            return hotel?.Id > 0;
        }
    }
}
