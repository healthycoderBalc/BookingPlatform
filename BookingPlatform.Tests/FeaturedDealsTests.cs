using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Features.FeaturedDeal.Queries.GetFeaturedHotels;
using BookingPlatform.Application.Features.FeaturedHotel.Queries.GetFeaturedHotels;
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
    public class FeaturedDealsTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IFeaturedDealRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetFeaturedHotelsQueryHandler>> _loggerMock;
        private readonly GetFeaturedHotelsQueryHandler _handler;
        private readonly GetFeaturedHotelsValidator _validator;

        public FeaturedDealsTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IFeaturedDealRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetFeaturedHotelsQueryHandler>>>();


            _handler = new GetFeaturedHotelsQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            _validator = new GetFeaturedHotelsValidator();
        }

        [Fact]
        public async Task GetFeaturedDeals_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Create<GetFeaturedHotelsQuery>();

            _repositoryMock.Setup(r => r.GetFeaturedDealsAsync())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }


        [Fact]
        public async Task GetFeaturedDeals_ValidRequests_ReturnsSuccessResponseWithFeaturedHotels()
        {
            // Arrange
            var request = _fixture.Create<GetFeaturedHotelsQuery>();

            var featuredDeals = _fixture.CreateMany<FeaturedDeal>().ToList();
            var featuredHotelDtos = _fixture.CreateMany<FeaturedHotelDto>().ToList();

            _repositoryMock.Setup(r => r.GetFeaturedDealsAsync())
                .ReturnsAsync(featuredDeals);

            _mapperMock.Setup(m => m.Map<ICollection<FeaturedHotelDto>>(featuredDeals))
                .Returns(featuredHotelDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(featuredHotelDtos, response.FeaturedHotels);
        }
    }
}