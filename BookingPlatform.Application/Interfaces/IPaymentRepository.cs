using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Application.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public new Task<Payment> AddAsync(Payment payment);
    }
}
