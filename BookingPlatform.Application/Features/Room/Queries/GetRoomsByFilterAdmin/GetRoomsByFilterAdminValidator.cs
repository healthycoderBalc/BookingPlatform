using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin
{
    public class GetRoomsByFilterAdminValidator : AbstractValidator<GetRoomsByFilterAdminQuery>
    {
        public GetRoomsByFilterAdminValidator()
        {
            RuleFor(r => r.RoomFilter.AdultCapacity)
              .GreaterThanOrEqualTo(0)
              .WithMessage("Number of adults must be a non-negative number.");

            RuleFor(r => r.RoomFilter.ChildrenCapacity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of children cannot be negative.");

            RuleFor(x => x.RoomFilter.CreationDate)
               .LessThanOrEqualTo(DateTime.Now)
               .WithMessage("Creation date must be in the past.")
               .When(x => x.RoomFilter.CreationDate.HasValue);

            RuleFor(x => x.RoomFilter.ModificationDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Modification date must be in the past.")
                .When(x => x.RoomFilter.ModificationDate.HasValue);
        }
    }
}
