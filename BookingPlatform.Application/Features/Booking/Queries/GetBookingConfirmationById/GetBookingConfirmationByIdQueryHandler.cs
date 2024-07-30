using AutoMapper;
using BookingPlatform.Application.Features.Booking.Commands.AddToCart;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById
{
    public class GetBookingConfirmationByIdQueryHandler : IRequestHandler<GetBookingConfirmationByIdQuery, GetBookingConfirmationByIdResponse>
    {
        private readonly IBookingRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBookingConfirmationByIdQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetBookingConfirmationByIdQueryHandler(IBookingRepository repository, IMapper mapper, ILogger<GetBookingConfirmationByIdQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetBookingConfirmationByIdResponse> Handle(GetBookingConfirmationByIdQuery request, CancellationToken cancellationToken)
        {
            var bookingConfirmationResponse = new GetBookingConfirmationByIdResponse();
            var validator = new GetBookingConfirmationByIdValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    bookingConfirmationResponse.Success = false;
                    bookingConfirmationResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        bookingConfirmationResponse.ValidationErrors.Add(error);
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return bookingConfirmationResponse;
                }
                else if (bookingConfirmationResponse.Success)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        bookingConfirmationResponse.Success = false;
                        bookingConfirmationResponse.Message = "User not authenticated";
                        return bookingConfirmationResponse;
                    }
                    var result = await _repository.GetBookingConfirmationAsync(request.Id);
                    if (result.UserId != userId)
                    {
                        bookingConfirmationResponse.Success = false;
                        bookingConfirmationResponse.Message = "Booking does not belong to authenticated user";
                        return bookingConfirmationResponse;
                    }
                    bookingConfirmationResponse.booking = _mapper.Map<BookingConfirmationDto>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                bookingConfirmationResponse.Success = false;
                bookingConfirmationResponse.Message = ex.Message;
            }

            return bookingConfirmationResponse;
        }
    }
}
