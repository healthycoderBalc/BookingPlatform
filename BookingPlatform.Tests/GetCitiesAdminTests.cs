using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCities;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetCitiesAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ICityRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetCitiesQueryHandler>> _loggerMock;
        private readonly GetCitiesQueryHandler _handler;

        public GetCitiesAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<ICityRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetCitiesQueryHandler>>>();


            _handler = new GetCitiesQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
             );
        }

        [Fact]
        public async Task GetCities_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture
                .Create<GetCitiesQuery>();

            _repositoryMock.Setup(r => r.GetCitiesAdminAsync())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }

        [Fact]
        public async Task GetCitiesAdmin_CorrectRequest_ReturnsCities()
        {
            // Arrange
            var query = _fixture
                .Create<GetCitiesQuery>();

            var cities = _fixture.CreateMany<(City, int)>()
                .ToList();
            var citiesAdminDto = _fixture.CreateMany<CityAdminDto>()
                .ToList();

            _repositoryMock.Setup(repo => repo.GetCitiesAdminAsync())
                .ReturnsAsync(cities);


            _mapperMock.Setup(m => m.Map<ICollection<CityAdminDto>>(cities))
                .Returns(citiesAdminDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(citiesAdminDto, response.CitiesAdmin);
        }
    }
}