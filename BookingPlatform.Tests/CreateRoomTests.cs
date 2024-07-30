using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Room.Commands.CreateRoom;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class CreateRoomTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRoomRepository> _repositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IRepository<RoomType>> _roomTypeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateRoomCommandHandler>> _loggerMock;
        private readonly CreateRoomCommandHandler _handler;

        public CreateRoomTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IRoomRepository>>();
            _hotelRepositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _roomTypeRepositoryMock = _fixture.Freeze<Mock<IRepository<RoomType>>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<CreateRoomCommandHandler>>>();


            _handler = new CreateRoomCommandHandler(
               _repositoryMock.Object, _hotelRepositoryMock.Object, _roomTypeRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateRoomAdmin_ValidRequests_ReturnsSuccessResponseWithRoomId()
        {
            // Arrange
            var roomId = 1;
            var hotel = _fixture.Build<Hotel>()
                .With(h => h.IsDeleted, false)
                .Create();

            _hotelRepositoryMock.Setup(r => r.GetByIdAsync(It.Is<int>(id => id == hotel.Id)))
                .ReturnsAsync(hotel);


            var type = _fixture.Build<RoomType>()
                .With(rt => rt.IsDeleted, false)
                .Create();

            _roomTypeRepositoryMock.Setup(r => r.GetByIdAsync(It.Is<int>(id => id == type.Id)))
                .ReturnsAsync(type);

            var roomCreationDto = _fixture.Build<RoomCreationDto>()
                .With(hf => hf.HotelId, hotel.Id)
                .With(hf => hf.TypeId, type.Id)
                .With(hf => hf.PricePerNight, 200)
                .With(hf => hf.AdultCapacity, 2)
                .With(hf => hf.ChildrenCapacity, 0)
                .With(r => r.IsOperational, true)
                .Create();

            var command = _fixture.Build<CreateRoomCommand>()
               .With(q => q.CreateRoom, roomCreationDto)
               .Create();

            var room = _fixture.Build<Room>()
             .With(r => r.Id, roomId)
             .With(hf => hf.HotelId, hotel.Id)
             .With(hf => hf.TypeId, type.Id)
             .With(hf => hf.PricePerNight, 200)
             .With(hf => hf.AdultCapacity, 2)
             .With(hf => hf.ChildrenCapacity, 0)
             .With(hf => hf.IsDeleted, false)
             .With(r => r.IsOperational, true)
             .Create();

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Room>())).ReturnsAsync(room);

            _mapperMock.Setup(m => m.Map<Room>(It.Is<RoomCreationDto>(dto => dto == roomCreationDto))).Returns(room);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(room.Id, response.Id);
        }
    }
}