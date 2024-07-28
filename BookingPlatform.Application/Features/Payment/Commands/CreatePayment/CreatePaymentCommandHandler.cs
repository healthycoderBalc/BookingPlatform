using AutoMapper;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;

namespace BookingPlatform.Application.Features.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
    {
        private readonly IPaymentRepository _repository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IPdfService _pdfService;

        public CreatePaymentCommandHandler(IPaymentRepository repository, IBookingRepository bookingRepository, IMapper mapper, ILogger<CreatePaymentCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IPdfService pdfService)
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _pdfService = pdfService;
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

                    var pdfContent = GeneratePdfContent(bookingEntity);
                    var pdfBytes = _pdfService.GeneratePdf(pdfContent);

                    var result = await _repository.AddAsync(paymentEntity, pdfBytes);
                    paymentResponse.Id = result.Id;
                    var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

                    var emailSubject = "Your Booking Confirmation!";
                    var emailMessage = $"Dear {userEmail},\n\nYour payment for booking ID {bookingEntity.Id} has been received.\n\n" +
                    $"Confirmation Number: {bookingEntity.ConfirmationNumber}\n\nTotal Price: {bookingEntity.TotalPrice:C}\n\n" +
                                       "Thank you for booking with us.\n\nBest regards,\n\nBooking Platform";

                    await _emailService.SendEmailAsync(userEmail, emailSubject, emailMessage);
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


        private string GeneratePdfContent(Domain.Entities.Booking booking)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Payment Confirmation");
            sb.AppendLine();
            sb.AppendLine($"Your payment for booking ID {booking.Id} has been received.");
            sb.AppendLine();
            sb.AppendLine($"Confirmation Number: {booking.ConfirmationNumber}");
            sb.AppendLine();
            sb.AppendLine($"Total Price: {booking.TotalPrice:C}");
            sb.AppendLine();
            sb.AppendLine($"Hotel: {booking.Room.Hotel.Name}");
            sb.AppendLine($"Address: {booking.Room.Hotel.Address}");
            sb.AppendLine($"Room Number: {booking.Room.RoomNumber}");
            sb.AppendLine($"Room Type: {booking.Room.Type.Name}");

            return sb.ToString();
        }

    }
}
