using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public Task<Payment> AddAsync(Payment payment, byte[] pdfBytes);
    }
}
