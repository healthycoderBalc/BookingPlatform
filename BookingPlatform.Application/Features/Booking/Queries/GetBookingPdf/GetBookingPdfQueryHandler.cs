using AutoMapper;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf
{
    public class GetBookingPdfQueryHandler : IRequestHandler<GetBookingPdfQuery, GetBookingPdfResponse>
    {
        private readonly IBookingRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBookingPdfQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetBookingPdfQueryHandler(IBookingRepository repository, IMapper mapper, ILogger<GetBookingPdfQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetBookingPdfResponse> Handle(GetBookingPdfQuery request, CancellationToken cancellationToken)
        {
            var bookingPdfResponse = new GetBookingPdfResponse();
            var validator = new GetBookingPdfValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    bookingPdfResponse.Success = false;
                    bookingPdfResponse.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        _logger.LogError($"Validation failed due to error- {error}");
                    }
                    return bookingPdfResponse;
                }
                else if (bookingPdfResponse.Success)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        bookingPdfResponse.Success = false;
                        bookingPdfResponse.Message = "User not authenticated";
                        return bookingPdfResponse;
                    }
                    var result = await _repository.GetByIdAsync(request.Id);
                    if (result.UserId != userId)
                    {
                        bookingPdfResponse.Success = false;
                        bookingPdfResponse.Message = "Booking does not belong to authenticated user";
                        return bookingPdfResponse;
                    }

                    if (result == null || result.PdfBytes == null)
                    {
                        bookingPdfResponse.Success = false; 
                        bookingPdfResponse.Message = "Booking confirmation PDF not found.";
                        return bookingPdfResponse;
                    }

                    bookingPdfResponse.PdfBytes = result.PdfBytes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while due to error- {ex.Message}");
                bookingPdfResponse.Success = false;
                bookingPdfResponse.Message = ex.Message;
            }

            return bookingPdfResponse;
        }
    }
}
