using AutoMapper;
using BookingPlatform.Application.Features.Payment.Dtos;

namespace BookingPlatform.Application.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Domain.Entities.Payment, CreatePaymentDto>()
               .ReverseMap();
        }
    }
}
