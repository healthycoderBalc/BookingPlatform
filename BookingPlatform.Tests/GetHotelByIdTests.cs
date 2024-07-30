using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.Hotel.Dtos;
using BookingPlatform.Application.Features.Hotel.Queries.GetHotelById;
using BookingPlatform.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class GetHotelByIdTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelByIdQueryHandler>> _loggerMock;
        private readonly GetHotelByIdQueryHandler _handler;

        public GetHotelByIdTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetHotelByIdQueryHandler>>>();


            _handler = new GetHotelByIdQueryHandler(
               _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetHotelById_HotelNotExist_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetHotelByIdQuery { Id = 999 };
            var validator = new GetHotelByIdValidator(_repositoryMock.Object);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Hotel)null);

            // Act
            var validationResult = await validator.ValidateAsync(request);
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, x => x.ErrorMessage == "Hotel does not exist");
        }

        [Fact]
        public async Task GetHotelById_NegativeHotelId_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetHotelByIdQuery { Id = -1 };
            var validator = new GetHotelByIdValidator(_repositoryMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(request);
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, x => x.ErrorMessage == "Id greater than 0");
        }


        [Fact]
        public async Task GetHotelById_ValidRequests_ReturnsSuccessResponseWithRequestedHotel()
        {
            // Arrange
            var query = new GetHotelByIdQuery { Id = 1 };
            var hotel = new Domain.Entities.Hotel { Id = 1, IsDeleted = false, HotelReviews = new List<Domain.Entities.HotelReview>() };
            var hotelReviewsDto = new List<HotelReviewDto>();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(hotel);

            _mapperMock.Setup(m => m.Map<ICollection<HotelReviewDto>>(It.IsAny<IEnumerable<Domain.Entities.HotelReview>>()))
                .Returns(hotelReviewsDto);

            var validator = new GetHotelByIdValidator(_repositoryMock.Object);
            var validationResult = await validator.ValidateAsync(query);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.True(response.Success);
            Assert.Equal(hotelReviewsDto, response.HotelReviews);
        }
    }
}