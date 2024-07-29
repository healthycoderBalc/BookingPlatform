using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelsBySearch;
using BookingPlatform.Application.Features.User.Commands.LoginUser;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata;

namespace BookingPlatform.Tests
{
    public class HotelsSearchTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelsBySearchQueryHandler>> _loggerMock;
        private readonly GetHotelsBySearchQueryHandler _handler;

        public HotelsSearchTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetHotelsBySearchQueryHandler>>>();


            _handler = new GetHotelsBySearchQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task SearchHotels_InvalidAdultsRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.Adults,0)
                                  .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Number of adults must be at least 1.", response.ValidationErrors);
        }

        [Fact]
        public async Task SearchHotels_InvalidChildrenRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.Children, -1)
                                  .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Number of children cannot be negative.", response.ValidationErrors);
        }

        [Fact]
        public async Task SearchHotels_InvalidDatesRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.CheckIn, DateTime.Now.AddDays(2))
                                  .With(q => q.CheckOut, DateTime.Now.AddDays(1))
                                  .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("CheckIn date must be before or equal to CheckOut date.", response.ValidationErrors);
        }

        [Fact]
        public async Task SearchHotels_OnlyCheckinDateRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.CheckIn, DateTime.Now.AddDays(2))
                                  .With(q => q.CheckOut, (DateTime?)null)
                                  .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("CheckOut date must be provided if CheckIn date is specified.", response.ValidationErrors);
        }

        [Fact]
        public async Task SearchHotels_OnlyCheckoutDateRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.CheckIn, (DateTime?)null)
                                  .With(q => q.CheckOut, DateTime.Now.AddDays(2))
                                  .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("CheckIn date must be provided if CheckOut date is specified.", response.ValidationErrors);
        }

        [Fact]
        public async Task SearchHotels_ValidRequests_ReturnsSuccessResponseWithHotels()
        {
            // Arrange
            var request = _fixture.Build<GetHotelsBySearchQuery>()
                                  .With(q => q.CheckIn, DateTime.Now.AddDays(1))
                                  .With(q => q.CheckOut, DateTime.Now.AddDays(2))
                                  .Create();

            var hotels = _fixture.CreateMany<Hotel>().ToList();
            var hotelDtos = _fixture.CreateMany<HotelDto>().ToList();

            _repositoryMock.Setup(r => r.GetHotelsBySearchAsync(
                    request.HotelName,
                    request.CityName,
                    request.CheckIn,
                    request.CheckOut,
                    request.Adults,
                    request.Children))
                .ReturnsAsync(hotels);

            _mapperMock.Setup(m => m.Map<ICollection<HotelDto>>(hotels))
                .Returns(hotelDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(hotelDtos, response.Hotels);
        }
    }
}