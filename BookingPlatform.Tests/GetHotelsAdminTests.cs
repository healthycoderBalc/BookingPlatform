using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Booking.Queries.GetBookingConfirmationById;
using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotels;
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
    public class GetHotelsAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelsQueryHandler>> _loggerMock;
        private readonly GetHotelsQueryHandler _handler;

        public GetHotelsAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetHotelsQueryHandler>>>();


            _handler = new GetHotelsQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
             );
        }

        [Fact]
        public async Task GetHotels_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture
                .Create<GetHotelsQuery>();

            _repositoryMock.Setup(r => r.GetHotelsAdminAsync())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }

        [Fact]
        public async Task GetHotelsAdmin_CorrectRequest_ReturnsHotels()
        {
            // Arrange
            var query = _fixture
                .Create<GetHotelsQuery>();

            var hotels = _fixture.CreateMany<(Hotel, int)>()
                .ToList();
            var hotelsAdminDto = _fixture.CreateMany<HotelAdminDto>()
                .ToList();

            _repositoryMock.Setup(repo => repo.GetHotelsAdminAsync())
                .ReturnsAsync(hotels);


            _mapperMock.Setup(m => m.Map<ICollection<HotelAdminDto>>(hotels))
                .Returns(hotelsAdminDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(hotelsAdminDto, response.HotelsAdmin);
        }
    }
}