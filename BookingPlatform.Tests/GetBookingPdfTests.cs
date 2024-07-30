using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingPdf;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingPlatform.Tests
{
    public class GetBookingPdfTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBookingRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetBookingPdfQueryHandler>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly GetBookingPdfQueryHandler _handler;

        public GetBookingPdfTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IBookingRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetBookingPdfQueryHandler>>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            _httpContextMock = _fixture.Freeze<Mock<HttpContext>>();


            _handler = new GetBookingPdfQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object
             );
        }

        [Fact]
        public async Task GetBookingPdf_NotBelongToUnauthenticatedUser_ReturnsFailureResponse()
        {
            // Arrange
            var query = _fixture
                .Build<GetBookingPdfQuery>()
                .With(r => r.Id, 1)
                .Create<GetBookingPdfQuery>();

            var userAuth = _fixture.Build<User>()
                 .With(u => u.Id, "user123")
                 .Create();

            var differentUserAuth = _fixture.Build<User>()
             .With(u => u.Id, "differentUser123")
             .Create();

            var booking = _fixture.Build<Booking>()
               .With(c => c.Id, query.Id)
               .With(c => c.IsDeleted, false)
               .With(c => c.UserId, differentUserAuth.Id)
               .Create();

            _repositoryMock.Setup(r => r.GetByIdAsync(query.Id))
                .ReturnsAsync(booking);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                  {
                            new Claim(ClaimTypes.NameIdentifier, userAuth.Id)
                  }, "test-auth-type"));

            _httpContextMock.SetupGet(c => c.User).Returns(claimsPrincipal);
            _httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Booking does not belong to authenticated user", response.Message);
        }

        [Fact]
        public async Task GetBookingPdf_CorrectRequest_ReturnsBookingPdf()
        {
            // Arrange
            var query = _fixture.Build<GetBookingPdfQuery>()
                .With(r => r.Id, 1)
                .Create();

            var user = _fixture.Build<User>()
                 .With(u => u.Id, "user123")
                 .Create();

            var pdfResponse = _fixture.Build<byte[]>()
                .Create();

            var booking = _fixture.Build<Booking>()
                .With(c => c.Id, query.Id)
                .With(c => c.IsDeleted, false)
                .With(c => c.UserId, user.Id)
                .With(c => c.PdfBytes, pdfResponse)
                .Create();


            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(booking);

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
            Assert.Equal(booking.PdfBytes, response.PdfBytes);
        }
    }
}