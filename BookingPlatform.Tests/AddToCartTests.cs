using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Booking.Commands.AddToCart;
using BookingPlatform.Application.Features.Booking.Dtos;
using BookingPlatform.Application.Features.Room.Dtos;
using BookingPlatform.Application.Features.Room.Queries.GetRoomsByHotelId;
using BookingPlatform.Application.Features.User.Commands.LoginUser;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BookingPlatform.Tests
{
    public class AddToCartTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBookingRepository> _repositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddToCartCommandHandler>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly AddToCartCommandHandler _handler;

        public AddToCartTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IBookingRepository>>();
            _roomRepositoryMock = _fixture.Freeze<Mock<IRoomRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<AddToCartCommandHandler>>>();
            _httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            _httpContextMock = _fixture.Freeze<Mock<HttpContext>>();


            _handler = new AddToCartCommandHandler(
                _repositoryMock.Object,
                _roomRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object
             );
        }

        [Fact]
        public async Task AddToCart_NonExistentRoom_ReturnsFailureResponse()
        {
            //Arrange
            var command = _fixture.Create<AddToCartCommand>();
            command.AddToCartBooking.RoomId = 999;

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Room)null);

            //Act
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(response.Success);
            Assert.Contains("Room does not exist", response.ValidationErrors);
        }


        [Fact]
        public async Task AddToCart_NegativeRoomNumber_ReturnsFailureResponse()
        {
            //Arrange
            var command = _fixture.Create<AddToCartCommand>();
            command.AddToCartBooking.RoomId = -1;
            //var validator = new AddToCartValidator(_roomRepositoryMock.Object);

            //Act
            //var validationResult = await validator.ValidateAsync(command);
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(response.Success);
            Assert.Contains("Add To Cart Booking Room Id greater than 0", response.ValidationErrors);
        }

        [Fact]
        public async Task AddToCart_CheckInDateAfterCheckoutDate_ReturnsFailureResponse()
        {
            var checkinDate = DateTime.Now.AddDays(4);
            var checkOutDate = DateTime.Now.AddDays(1);
            // Arrange
            var command = _fixture.Create<AddToCartCommand>();

            command.AddToCartBooking.CheckInDate = checkinDate;
            command.AddToCartBooking.CheckOutDate = checkOutDate;
            command.AddToCartBooking.RoomId = 1;

            var room = _fixture.Build<Room>()
                .With(r => r.IsDeleted, false)
                .With(r => r.Id, 1)
                .Create();

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(room);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Contains("CheckIn date must be before or equal to CheckOut date.", response.ValidationErrors);
        }

        [Fact]
        public async Task AddToCart_CorrectBookingData_ReturnsCreatedBookingId()
        {
            // Arrange
            var checkinDate = DateTime.Now.AddDays(1);
            var checkOutDate = DateTime.Now.AddDays(4);
            var command = _fixture.Create<AddToCartCommand>();

            command.AddToCartBooking.CheckInDate = checkinDate;
            command.AddToCartBooking.CheckOutDate = checkOutDate;
            command.AddToCartBooking.RoomId = 1;

            var room = _fixture.Build<Room>()
                .With(r => r.IsDeleted, false)
                .With(r => r.Id, 1)
                .With(r => r.PricePerNight, 100)
                .Create();

            var user = _fixture.Build<User>()
                .With(u => u.Id, "user123")
                .Create();

            var booking = _fixture.Build<Booking>()
                .With(b => b.RoomId, room.Id)
                .With(b => b.IsDeleted, false)
                .With(b => b.IsConfirmed, false)
                .With(b => b.CheckInDate, checkinDate)
                .With(b => b.CheckOutDate, checkOutDate)
                .With(b => b.User, user)
                .With(b => b.UserId, user.Id)
                .With(b => b.Id, 3)
                .Create();

            var addToCartBookingDto = _fixture.Build<AddToCartBookingDto>()
                .With(b => b.RoomId, room.Id)
                .With(b => b.CheckInDate, checkinDate)
                .With(b => b.CheckOutDate, checkOutDate)
                .Create();

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(room);

            _repositoryMock.Setup(repo => repo.AddToCartAsync(It.IsAny<Booking>(), user.Id))
                .ReturnsAsync(booking);

            _mapperMock.Setup(m => m.Map<AddToCartBookingDto>(It.IsAny<Domain.Entities.Booking>()))
                .Returns(addToCartBookingDto);


            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }, "test-auth-type"));

            _httpContextMock.SetupGet(c => c.User).Returns(claimsPrincipal);
            _httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(_httpContextMock.Object);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(3, response.Id);
        }
    }
}