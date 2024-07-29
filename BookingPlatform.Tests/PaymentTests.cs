using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Payment.Commands.CreatePayment;
using BookingPlatform.Application.Features.Payment.Dtos;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace BookingPlatform.Tests
{
    public class PaymentTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPaymentRepository> _repositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreatePaymentCommandHandler>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IPdfService> _pdfServiceMock;
        private readonly CreatePaymentCommandHandler _handler;

        public PaymentTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IPaymentRepository>>();
            _bookingRepositoryMock = _fixture.Freeze<Mock<IBookingRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<CreatePaymentCommandHandler>>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            _httpContextMock = _fixture.Freeze<Mock<HttpContext>>();
            _emailServiceMock = _fixture.Freeze<Mock<IEmailService>>();
            _pdfServiceMock = _fixture.Freeze<Mock<IPdfService>>();


            _handler = new CreatePaymentCommandHandler(
                _repositoryMock.Object,
                _bookingRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object,
                _emailServiceMock.Object,
                _pdfServiceMock.Object
             );
        }

        [Fact]
        public async Task CreatePayment_NonExistentBooking_ReturnsFailureResponse()
        {
            //Arrange
            var command = _fixture.Create<CreatePaymentCommand>();
            command.CreatePayment.BookingId = 999;

            _bookingRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Booking)null);

            //Act
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(response.Success);
            Assert.Contains("Booking does not exist", response.ValidationErrors);
        }


        [Fact]
        public async Task CreatePayment_NegativeBookingNumber_ReturnsFailureResponse()
        {
            //Arrange
            var command = _fixture.Create<CreatePaymentCommand>();
            command.CreatePayment.BookingId = -1;

            //Act
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(response.Success);
            Assert.Contains("Create Payment Booking Id greater than 0", response.ValidationErrors);
        }

        [Fact]
        public async Task CreatePayment_CorrectPaymentData_ReturnsCreatedPaymentId()
        {
            // Arrange
            var bookingId = 1;
            var user = _fixture.Build<User>()
                .With(u => u.Id, "user123")
                .Create();

            var booking = _fixture.Build<Booking>()
                .With(b => b.Id, bookingId)
                .With(b => b.IsDeleted, false)
                .With(b => b.IsConfirmed, false)
                .With(b => b.User, user)
                .With(b => b.UserId, user.Id)
                .Create();

            _bookingRepositoryMock.Setup(repo => repo.GetByIdAsync(It.Is<int>(id => id == bookingId)))
                .Callback<int>(id => Console.WriteLine($"Mock GetByIdAsync called with ID: {id}"))
                .ReturnsAsync(booking);

            var createPaymentDto = _fixture.Build<CreatePaymentDto>()
                .With(cp => cp.BookingId, booking.Id)
                .With(cp => cp.PaymentMethod, "Credit Card")
                .With(cp => cp.PaymentStatus, "Pending")
                .Create();

            var command = _fixture.Create<CreatePaymentCommand>();
            command.CreatePayment = createPaymentDto;

            var payment = _fixture.Build<Payment>()
                .With(b => b.BookingId, booking.Id)
                .With(b => b.IsDeleted, false)
                .With(b => b.Booking, booking)
                .With(p => p.Id, 123)
                .Create();

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Payment>(), It.IsAny<byte[]>()))
                .ReturnsAsync(payment);

            _mapperMock.Setup(m => m.Map<Payment>(It.Is<CreatePaymentDto>(dto => dto == createPaymentDto)))
              .Returns(payment);


            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }, "test-auth-type"));

            _httpContextMock.SetupGet(c => c.User).Returns(claimsPrincipal);
            _httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(payment.Id, response.Id);
        }
    }
}