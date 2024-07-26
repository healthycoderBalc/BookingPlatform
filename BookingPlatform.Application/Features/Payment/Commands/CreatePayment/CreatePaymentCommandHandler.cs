using AutoMapper;
using BookingPlatform.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BookingPlatform.Application.Features.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
    {
        private readonly IPaymentRepository _repository;
        private readonly IRepository<Domain.Entities.Booking> _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatePaymentCommandHandler(IPaymentRepository repository, IRepository<Domain.Entities.Booking> bookingRepository, IMapper mapper, ILogger<CreatePaymentCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentResponse = new CreatePaymentResponse();
            var validator = new CreatePaymentValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    paymentResponse.Success = false;
                    paymentResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return paymentResponse;
                }
                else if (paymentResponse.Success)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        paymentResponse.Success = false;
                        paymentResponse.Message = "User not authenticated";
                        return paymentResponse;
                    }
                    var paymentEntity = _mapper.Map<Domain.Entities.Payment>(request.CreatePayment);
                    var bookingEntity = await _bookingRepository.GetByIdAsync(paymentEntity.BookingId);
                    if (bookingEntity.UserId != userId)
                    {
                        paymentResponse.Success = false;
                        paymentResponse.Message = "Booking does not belong to authenticated user";
                        return paymentResponse;
                    }
                    var result = await _repository.AddAsync(paymentEntity);
                    paymentResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                paymentResponse.Success = false;
                paymentResponse.Message = ex.Message;
            }

            return paymentResponse;
        }
    }
}
