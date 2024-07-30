using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRoomsByFilterAdmin;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetRoomsByFilterAdminTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRoomRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetRoomsByFilterAdminQueryHandler>> _loggerMock;
        private readonly GetRoomsByFilterAdminQueryHandler _handler;

        public GetRoomsByFilterAdminTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IRoomRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetRoomsByFilterAdminQueryHandler>>>();


            _handler = new GetRoomsByFilterAdminQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public async Task GetRoomsByFilterAdmin_NegativeNumberOfAdultsAndChildrenRequest_ReturnsFailureResponse(int starRating)
        {
            // Arrange

            var roomFilter = _fixture.Build<RoomFilterDto>()
                .With(hf => hf.AdultCapacity, -1)
                .With(hf => hf.ChildrenCapacity, -1)
                .Create();
            var request = _fixture.Build<GetRoomsByFilterAdminQuery>()
                .With(q => q.RoomFilter, roomFilter)
                .Create();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("Number of adults must be a non-negative number.", response.ValidationErrors);
            Assert.Contains("Number of children cannot be negative.", response.ValidationErrors);
        }


        [Fact]
        public async Task GetRoomsByFilterAdmin_DatesInFutureRequest_ReturnsFailureResponse()
        {
            // Arrange
            var roomFilter = _fixture.Build<RoomFilterDto>()
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(1))
                .Create();
            var request = _fixture.Build<GetRoomsByFilterAdminQuery>()
                .With(q => q.RoomFilter, roomFilter)
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
        public async Task GetRoomsByFilterAdmin_ValidRequests_ReturnsSuccessResponseWithFilteredRooms(int numberOfChildrenOrAdult)
        {
            // Arrange
            var roomFilter = _fixture.Build<RoomFilterDto>()
                .With(hf => hf.AdultCapacity, numberOfChildrenOrAdult)
                .With(hf => hf.ChildrenCapacity, numberOfChildrenOrAdult)
                .With(hf => hf.CreationDate, DateTime.Now.AddDays(-1))
                .With(hf => hf.ModificationDate, DateTime.Now.AddDays(-1))
                .Create();

            var request = _fixture.Build<GetRoomsByFilterAdminQuery>()
                .With(q => q.RoomFilter, roomFilter)
                .Create();

            var rooms = _fixture.Build<Room>()
                .CreateMany().ToList();

            var roomsAdmin = _fixture.Build<RoomAdminDto>()
                .CreateMany().ToList();

            _repositoryMock.Setup(repo => repo.GetRoomsByFilterAdminAsync(roomFilter.RoomNumber, roomFilter.Availability, roomFilter.AdultCapacity, roomFilter.ChildrenCapacity, roomFilter.CreationDate, roomFilter.ModificationDate)).ReturnsAsync(rooms);

            _mapperMock.Setup(m => m.Map<ICollection<RoomAdminDto>>(rooms)).Returns(roomsAdmin);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(roomsAdmin, response.FilteredRooms);
        }
    }
}