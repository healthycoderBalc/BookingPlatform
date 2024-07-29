using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.FilterHotelsSearch;
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
    public class FilterHotelsSearchTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<FilterHotelsSearchQueryHandler>> _loggerMock;
        private readonly FilterHotelsSearchQueryHandler _handler;

        public FilterHotelsSearchTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<FilterHotelsSearchQueryHandler>>>();


            _handler = new FilterHotelsSearchQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task FilterHotelsSearch_NoneHotelIdRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<FilterHotelsSearchQuery>()
                .With(q => q.HotelIds, [])
                .With(q => q.MinStarRating, 1)
                .With(q => q.MaxStarRating, 5)
                .With(q => q.MinPrice, 100)
                .With(q => q.MaxPrice, 200)
                .Create();
      
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("HotelIds must contain at least one hotel ID.", response.ValidationErrors);
        }


        [Fact]
        public async Task FilterHotelsSearch_NegativePricesRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<FilterHotelsSearchQuery>()
                .With(q => q.MinStarRating, 1)
                .With(q => q.MaxStarRating, 5)
                .With(q => q.MinPrice, -100)
                .With(q => q.MaxPrice, -50)
                .Create();
        
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Minimum price must be greater than or equal to 0.", response.ValidationErrors);
            Assert.Contains("Maximum price must be greater than or equal to 0.", response.ValidationErrors);
        }


        [Fact]
        public async Task FilterHotelsSearch_InvalidStarRatingRequest_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<FilterHotelsSearchQuery>()
                .With(q => q.MinStarRating, 0)
                .With(q => q.MaxStarRating, 6)
                .With(q => q.MinPrice, 100)
                .With(q => q.MaxPrice, 200)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Minimum star rating must be between 1 and 5.", response.ValidationErrors);
            Assert.Contains("Maximum star rating must be between 1 and 5.", response.ValidationErrors);
        }

        [Fact]
        public async Task FilterHotelsSearch_CollectionOfAmenitiesAndRoomTypesWithEmptyAndNullValues_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Build<FilterHotelsSearchQuery>()
                .With(q => q.Amenities, ["", null])
                .With(q => q.RoomTypes, [""])
                .With(q => q.MinStarRating, 1)
                .With(q => q.MaxStarRating, 5)
                .With(q => q.MinPrice, 100)
                .With(q => q.MaxPrice, 200)
                .Create();
          

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Amenities list cannot contain null or empty values.", response.ValidationErrors);
            Assert.Contains("Room types list cannot contain null or empty values.", response.ValidationErrors);
        }

        [Fact]
        public async Task FilterHotelsSearch_ValidRequests_ReturnsSuccessResponseWithFilteredHotels()
        {
            // Arrange
            var request = _fixture.Build<FilterHotelsSearchQuery>()
                .With(q => q.MinStarRating, 1)
                .With(q => q.MaxStarRating, 5)
                .With(q => q.MinPrice, 100)
                .With(q => q.MaxPrice, 200)
                .Create();
            var filteredHotels = _fixture.CreateMany<(Hotel Hotel, ICollection<decimal> PricesPerNight)>().ToList();
            var hotelDtos = _fixture.CreateMany<FilterHotelDto>().ToList();

            var result = (filteredHotels, filteredHotels.Count, filteredHotels.Count);

            _repositoryMock.Setup(repo => repo.FilterHotelsAsync(
                request.HotelIds,
                request.MinPrice,
                request.MaxPrice,
                request.MinStarRating,
                request.MaxStarRating,
                request.Amenities,
                request.RoomTypes,
                request.PageNumber,
                request.PageSize))
                    .ReturnsAsync(result);

            _mapperMock.Setup(m => m.Map<ICollection<FilterHotelDto>>(It.IsAny<IEnumerable<(Hotel, ICollection<decimal>)>>()))
                .Returns(hotelDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(hotelDtos, response.FilteredHotels);
            Assert.Equal(filteredHotels.Count, response.TotalCount);
            Assert.Equal(filteredHotels.Count, response.TotalCountThisPage);
        }
    }
}