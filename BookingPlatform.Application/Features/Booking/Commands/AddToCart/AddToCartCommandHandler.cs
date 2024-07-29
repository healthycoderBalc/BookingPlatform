using AutoMapper;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookingPlatform.Application.Features.Booking.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, AddToCartResponse>
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddToCartCommandHandler(IBookingRepository repository, IRoomRepository roomRepository, IMapper mapper, ILogger<AddToCartCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AddToCartResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var addToCartResponse = new AddToCartResponse();
            var validator = new AddToCartValidator(_roomRepository);

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    addToCartResponse.Success = false;
                    addToCartResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return addToCartResponse;
                }
                else if (addToCartResponse.Success)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        addToCartResponse.Success = false;
                        addToCartResponse.Message = "User not authenticated";
                        return addToCartResponse;
                    }
                    var bookingEntity = _mapper.Map<Domain.Entities.Booking>(request.AddToCartBooking);
                    var result = await _repository.AddToCartAsync(bookingEntity, userId);
                    addToCartResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                addToCartResponse.Success = false;
                addToCartResponse.Message = ex.Message;
            }

            return addToCartResponse;

        }
    }
}
