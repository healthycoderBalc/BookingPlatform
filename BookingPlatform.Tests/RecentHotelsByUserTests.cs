using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetRecentHotelsByUser;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace BookingPlatform.Tests
{
    public class RecentHotelsByUserTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetRecentHotelsByUserQueryHandler>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly GetRecentHotelsByUserQueryHandler _handler;

        public RecentHotelsByUserTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetRecentHotelsByUserQueryHandler>>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();


            _handler = new GetRecentHotelsByUserQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task GetRecentHotelsByUser_NoUserAuthenticated_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Create<GetRecentHotelsByUserQuery>();

            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(context);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Null(response.RecentHotels);
        }


        [Fact]
        public async Task GetRecentHotelsByUser_ValidRequests_ReturnsSuccessResponseWithRecentHotels()
        {
            // Arrange
            var request = _fixture.Create<GetRecentHotelsByUserQuery>();
            var userId = "test-user-id";
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };
            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(context);

            var recentHotelRooms = _fixture.CreateMany<(Hotel, Room)>().ToList();
            var recentHotelDtos = _fixture.CreateMany<RecentHotelDto>().ToList();

            _repositoryMock.Setup(repo => repo.GetRecentHotelsByAuthUserAsync(userId))
                .ReturnsAsync(recentHotelRooms);

            _mapperMock.Setup(m => m.Map<ICollection<RecentHotelDto>>(It.IsAny<IEnumerable<(Hotel, Room)>>()))
                .Returns(recentHotelDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(recentHotelDtos, response.RecentHotels);
        }
    }
}