using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch
{
    public class FilterHotelsSearchValidator : AbstractValidator<FilterHotelsSearchQuery>
    {
        public FilterHotelsSearchValidator()
        {
            RuleFor(query => query.HotelIds)
               .NotNull().WithMessage("HotelIds cannot be null.")
               .Must(hotelIds => hotelIds.Any()).WithMessage("HotelIds must contain at least one hotel ID.");

            RuleFor(query => query.MinPrice)
              .GreaterThanOrEqualTo(0).When(query => query.MinPrice.HasValue)
              .WithMessage("Minimum price must be greater than or equal to 0.");

            RuleFor(query => query.MaxPrice)
                .GreaterThanOrEqualTo(0).When(query => query.MaxPrice.HasValue)
                .WithMessage("Maximum price must be greater than or equal to 0.");

            RuleFor(query => query)
                .Must(query => !query.MinPrice.HasValue || !query.MaxPrice.HasValue || query.MinPrice <= query.MaxPrice)
                .WithMessage("Minimum price cannot be greater than maximum price.");

            RuleFor(query => query.MinStarRating)
                .InclusiveBetween(1, 5).When(query => query.MinStarRating.HasValue)
                .WithMessage("Minimum star rating must be between 1 and 5.");

            RuleFor(query => query.MaxStarRating)
                .InclusiveBetween(1, 5).When(query => query.MaxStarRating.HasValue)
                .WithMessage("Maximum star rating must be between 1 and 5.");

            RuleFor(query => query)
                .Must(query => !query.MinStarRating.HasValue || !query.MaxStarRating.HasValue || query.MinStarRating <= query.MaxStarRating)
                .WithMessage("Minimum star rating cannot be greater than maximum star rating.");

            RuleFor(query => query.Amenities)
               .Must(amenities => amenities == null || amenities.All(a => !string.IsNullOrEmpty(a)))
               .WithMessage("Amenities list cannot contain null or empty values.");

            RuleFor(query => query.RoomTypes)
                .Must(roomTypes => roomTypes == null || roomTypes.All(rt => !string.IsNullOrEmpty(rt)))
                .WithMessage("Room types list cannot contain null or empty values.");
        }
    }
}
