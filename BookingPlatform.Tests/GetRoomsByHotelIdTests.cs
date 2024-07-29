using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.City.Queries.GetTrendingCities;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId;
using BookingPlatform.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetRoomsByHotelIdTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IRoomRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetRoomsByHotelIdQueryHandler>> _loggerMock;
        private readonly GetRoomsByHotelIdQueryHandler _handler;

        public GetRoomsByHotelIdTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IRoomRepository>>();
            _hotelRepositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetRoomsByHotelIdQueryHandler>>>();


            _handler = new GetRoomsByHotelIdQueryHandler(
               _repositoryMock.Object, _hotelRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetRoomsByHotelId_HotelNotExist_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetRoomsByHotelIdQuery { HotelId = 999 };
            var validator = new GetRoomsByHotelIdValidator(_hotelRepositoryMock.Object);

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Hotel)null);

            // Act
            var validationResult = await validator.ValidateAsync(request);
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, x => x.ErrorMessage == "Hotel does not exist");
        }

        [Fact]
        public async Task GetRoomsByHotelId_NegativeHotelId_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetRoomsByHotelIdQuery { HotelId = -1 };
            var validator = new GetRoomsByHotelIdValidator(_hotelRepositoryMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(request);
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, x => x.ErrorMessage == "Hotel Id greater than 0");
        }


        [Fact]
        public async Task GetRoomsByHotelId_ValidRequests_ReturnsSuccessResponseWithRequestedRooms()
        {
            // Arrange
            var query = _fixture
                .Build<GetRoomsByHotelIdQuery>()
                .With(q => q.HotelId, 1)
                .Create();

            var hotel = _fixture
                .Build<Domain.Entities.Hotel>()
                .With(h => h.IsDeleted, false)
                .With(h => h.Id, 1)
                .Create();

            ICollection<Domain.Entities.Room> rooms = _fixture
                .Build<Domain.Entities.Room>()
                .With(h => h.IsDeleted, false)
                .CreateMany().ToList();

            var detailedRoomDto = _fixture
                .Build<DetailedRoomDto>()
                .CreateMany().ToList();

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(hotel);

            _repositoryMock.Setup(repo => repo.GetByHotelIdAsync(It.IsAny<int>()))
                .ReturnsAsync(rooms);

            _mapperMock.Setup(m => m.Map<ICollection<DetailedRoomDto>>(It.IsAny<IEnumerable<Domain.Entities.Room>>()))
                .Returns(detailedRoomDto);

            var validator = new GetRoomsByHotelIdValidator(_hotelRepositoryMock.Object);
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.True(response.Success);
            Assert.Equal(detailedRoomDto, response.DetailedRooms);
        }
    }
}