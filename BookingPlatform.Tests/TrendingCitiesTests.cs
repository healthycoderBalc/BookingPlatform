using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetTrendingCities;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class TrendingCitiesTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ICityRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetTrendingCitiesQueryHandler>> _loggerMock;
        private readonly GetTrendingCitiesQueryHandler _handler;
        private readonly GetTrendingCitiesValidator _validator;

        public TrendingCitiesTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<ICityRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetTrendingCitiesQueryHandler>>>();


            _handler = new GetTrendingCitiesQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            _validator = new GetTrendingCitiesValidator();
        }

        [Fact]
        public async Task GetTrendingCities_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture.Create<GetTrendingCitiesQuery>();

            _repositoryMock.Setup(r => r.GetTrendingCitiesAsync())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }


        [Fact]
        public async Task GetTrendingCities_ValidRequests_ReturnsSuccessResponseWithTrendingCities()
        {
            // Arrange
            var request = _fixture.Create<GetTrendingCitiesQuery>();

            var cities = _fixture.CreateMany<(City, int)>().ToList();
            var trendingCityDtos = _fixture.CreateMany<TrendingCityDto>().ToList();

            _repositoryMock.Setup(r => r.GetTrendingCitiesAsync())
                .ReturnsAsync(cities);

            _mapperMock.Setup(m => m.Map<ICollection<TrendingCityDto>>(cities))
                .Returns(trendingCityDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(trendingCityDtos, response.TrendingCities);
        }
    }
}