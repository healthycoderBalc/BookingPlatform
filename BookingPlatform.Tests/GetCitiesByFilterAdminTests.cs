using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.City.Dtos;
using BookingPlatform.Application.Features.City.Queries.GetCitiesByFilter;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetCitiesByFilterAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ICityRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetCitiesByFilterQueryHandler>> _loggerMock;
        private readonly GetCitiesByFilterQueryHandler _handler;

        public GetCitiesByFilterAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<ICityRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetCitiesByFilterQueryHandler>>>();


            _handler = new GetCitiesByFilterQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetCitiesByFilterAdmin_NegativeNumberOfHotelsRequest_ReturnsFailureResponse()
        {
            // Arrange
            var cityFilter = _fixture.Build<CityFilterDto>()
                .With(hf => hf.NumberOfHotels, -2)
                .Create();
            var request = _fixture.Build<GetCitiesByFilterQuery>()
                .With(q => q.CityFilter, cityFilter)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Number of hotels must be a non-negative number.", response.ValidationErrors);
        }


        [Fact]
        public async Task GetCitiesByFilterAdmin_DatesInFutureRequest_ReturnsFailureResponse()
        {
            // Arrange
            var cityFilter = _fixture.Build<CityFilterDto>()
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(1))
                .Create();
            var request = _fixture.Build<GetCitiesByFilterQuery>()
                .With(q => q.CityFilter, cityFilter)
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
        public async Task GetCitiesByFilterAdmin_ValidRequests_ReturnsSuccessResponseWithFilteredCities(int numberOfHotels)
        {
            // Arrange
            var cityFilter = _fixture.Build<CityFilterDto>()
                .With(hf => hf.NumberOfHotels, numberOfHotels)
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(-1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(-1))
                .Create();

            var request = _fixture.Build<GetCitiesByFilterQuery>()
                .With(q => q.CityFilter, cityFilter)
                .Create();

            var cities = _fixture.Build<(City, int)>()
                .CreateMany().ToList();

            var citiesAdmin = _fixture.Build<CityAdminDto>()
                .CreateMany().ToList();

            _repositoryMock.Setup(repo => repo.GetCitiesByFilterAsync(cityFilter.Name, cityFilter.Country, cityFilter.PostalCode, cityFilter.NumberOfHotels, cityFilter.CreationDate, cityFilter.ModificationDate)).ReturnsAsync(cities);

            _mapperMock.Setup(m => m.Map<ICollection<CityAdminDto>>(cities)).Returns(citiesAdmin);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(citiesAdmin, response.FilteredCities);
        }
    }
}