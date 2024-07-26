using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(BookingPlatformDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<Payment> AddAsync(Payment payment)
        {
            var booking = await _dbContext.Bookings.FindAsync(payment.BookingId) ?? throw new Exception("Booking not found");

            payment.Amount = booking.TotalPrice;

            booking.IsConfirmed = true;
            booking.ConfirmationNumber = GenerateConfirmationNumber();

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        private string GenerateConfirmationNumber()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            Random random = new Random();

            char[] confirmationNumber = new char[6];
            for (int i = 0; i < 3; i++)
            {
                confirmationNumber[i] = letters[random.Next(letters.Length)];
            }
            for (int i = 3; i < 6; i++)
            {
                confirmationNumber[i] = numbers[random.Next(numbers.Length)];
            }

            return new string(confirmationNumber.OrderBy(c => random.Next()).ToArray());
        }
    }
}
