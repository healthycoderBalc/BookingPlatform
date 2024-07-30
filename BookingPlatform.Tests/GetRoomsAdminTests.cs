using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRooms;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetRoomsAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRoomRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetRoomsQueryHandler>> _loggerMock;
        private readonly GetRoomsQueryHandler _handler;

        public GetRoomsAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IRoomRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetRoomsQueryHandler>>>();


            _handler = new GetRoomsQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
             );
        }

        [Fact]
        public async Task GetRooms_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            var request = _fixture
                .Create<GetRoomsQuery>();

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("Repository failure", response.Message);
        }

        [Fact]
        public async Task GetRoomsAdmin_CorrectRequest_ReturnsRooms()
        {
            // Arrange
            var query = _fixture
                .Create<GetRoomsQuery>();

            var rooms = _fixture.CreateMany<Room>()
                .ToList();
            var roomsAdminDto = _fixture.CreateMany<RoomAdminDto>()
                .ToList();

            _repositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(rooms);


            _mapperMock.Setup(m => m.Map<ICollection<RoomAdminDto>>(rooms))
                .Returns(roomsAdminDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(roomsAdminDto, response.RoomsAdmin);
        }
    }
}