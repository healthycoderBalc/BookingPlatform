using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsByFilterAdmin;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetHotelsByFilterAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelsByFilterAdminQueryHandler>> _loggerMock;
        private readonly GetHotelsByFilterAdminQueryHandler _handler;

        public GetHotelsByFilterAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetHotelsByFilterAdminQueryHandler>>>();


            _handler = new GetHotelsByFilterAdminQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public async Task GetHotelsByFilterAdmin_InvalidStarRatingRequest_ReturnsFailureResponse(int starRating)
        {
            // Arrange

            var hotelFilter = _fixture.Build<HotelFilterDto>()
                .With(hf => hf.StarRating, starRating)
                .Create();
            var request = _fixture.Build<GetHotelsByFilterAdminQuery>()
                .With(q => q.HotelFilter, hotelFilter)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Hotel Filter Star Rating should have a value between 1 and 5", response.ValidationErrors);
        }


        [Fact]
        public async Task GetHotelsByFilterAdmin_NegativeNumberOfRoomsRequest_ReturnsFailureResponse()
        {
            // Arrange
            var hotelFilter = _fixture.Build<HotelFilterDto>()
                .With(hf => hf.NumberOfRooms, -2)
                .Create();
            var request = _fixture.Build<GetHotelsByFilterAdminQuery>()
                .With(q => q.HotelFilter, hotelFilter)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Number of hotels must be a non-negative number.", response.ValidationErrors);
        }


        [Fact]
        public async Task GetHotelsByFilterAdmin_DatesInFutureRequest_ReturnsFailureResponse()
        {
            // Arrange
            var hotelFilter = _fixture.Build<HotelFilterDto>()
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(1))
                .Create();
            var request = _fixture.Build<GetHotelsByFilterAdminQuery>()
                .With(q => q.HotelFilter, hotelFilter)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Creation date must be in the past.", response.ValidationErrors);
            Assert.Contains("Modification date must be in the past.", response.ValidationErrors);
        }

    
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task GetHotelsByFilterAdmin_ValidRequests_ReturnsSuccessResponseWithFilteredHotels(int starRating)
        {
            // Arrange
            var hotelFilter = _fixture.Build<HotelFilterDto>()
                .With(hf => hf.StarRating, starRating)
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(-1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(-1))
                .Create();

            var request = _fixture.Build<GetHotelsByFilterAdminQuery>()
                .With(q => q.HotelFilter, hotelFilter)
                .Create();

            var hotels = _fixture.Build<(Hotel, int)>()
          .CreateMany().ToList();

            var hotelsAdmin = _fixture.Build<HotelAdminDto>()
                .CreateMany().ToList();

            _repositoryMock.Setup(repo => repo.GetHotelsByFilterAdminAsync(hotelFilter.Name, hotelFilter.StarRating, hotelFilter.OwnerName, hotelFilter.NumberOfRooms, hotelFilter.CreationDate, hotelFilter.ModificationDate)).ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<ICollection<HotelAdminDto>>(hotels)).Returns(hotelsAdmin);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(hotelsAdmin, response.FilteredHotels);
        }
    }
}