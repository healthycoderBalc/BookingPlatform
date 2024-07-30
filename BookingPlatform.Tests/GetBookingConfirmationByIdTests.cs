using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById;
using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
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
    public class GetBookingConfirmationByIdTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBookingRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetBookingConfirmationByIdQueryHandler>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly GetBookingConfirmationByIdQueryHandler _handler;

        public GetBookingConfirmationByIdTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IBookingRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetBookingConfirmationByIdQueryHandler>>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            _httpContextMock = _fixture.Freeze<Mock<HttpContext>>();


            _handler = new GetBookingConfirmationByIdQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object
             );
        }

        [Fact]
        public async Task GetConfirmationById_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture
                .Build<GetBookingConfirmationByIdQuery>()
                .With(r => r.Id, 1)
                .Create<GetBookingConfirmationByIdQuery>();

            var user = _fixture.Build<User>()
                 .With(u => u.Id, "user123")
                 .Create();

            _repositoryMock.Setup(r => r.GetBookingConfirmationAsync(request.Id))
                .ThrowsAsync(new Exception("Repository failure"));
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                  {
                            new Claim(ClaimTypes.NameIdentifier, user.Id)
                  }, "test-auth-type"));

            _httpContextMock.SetupGet(c => c.User).Returns(claimsPrincipal);
            _httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }

        [Fact]
        public async Task GetConfirmationById_CorrectRequest_ReturnsBookingConfirmation()
        {
            // Arrange
            var query = _fixture.Build<GetBookingConfirmationByIdQuery>()
                .With(r => r.Id, 1)
                .Create();

            var user = _fixture.Build<User>()
                 .With(u => u.Id, "user123")
                 .Create();

            var booking = _fixture.Build<Booking>()
                .With(c => c.Id, query.Id)
                .With(c => c.IsDeleted, false)
                .With(c => c.UserId, user.Id)
                .Create();

            var bookingConfirmationDto = _fixture.Build<BookingConfirmationDto>()
                .With(bc => bc.Id, query.Id)
                .Create();

            _repositoryMock.Setup(repo => repo.GetBookingConfirmationAsync(It.IsAny<int>()))
                .ReturnsAsync(booking);

            _mapperMock.Setup(m => m.Map<BookingConfirmationDto>(It.IsAny<Booking>()))
                .Returns(bookingConfirmationDto);


            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "test-auth-type"));

            _httpContextMock.SetupGet(c => c.User).Returns(claimsPrincipal);
            _httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(bookingConfirmationDto, response.booking);
        }
    }
}