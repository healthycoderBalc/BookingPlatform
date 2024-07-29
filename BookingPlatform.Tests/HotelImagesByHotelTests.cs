using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BookingPlatform.Application.Features.FeaturedDeal.Dtos;
using BookingPlatform.Application.Features.HotelImages.Dtos;
using BookingPlatform.Application.Features.HotelImages.Queries.GetHotelImagesByHotelId;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingPlatform.Tests
{
    public class HotelImagesByHotelTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IHotelImageRepository> _repositoryMock;
        private readonly Mock<IRepository<Hotel>> _hotelRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelImagesByHotelIdQueryHandler>> _loggerMock;
        private readonly GetHotelImagesByHotelIdQueryHandler _handler;

        public HotelImagesByHotelTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMock = _fixture.Freeze<Mock<IHotelImageRepository>>();
            _hotelRepositoryMock = _fixture.Freeze<Mock<IRepository<Hotel>>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<GetHotelImagesByHotelIdQueryHandler>>>();


            _handler = new GetHotelImagesByHotelIdQueryHandler(
               _repositoryMock.Object, _hotelRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetHotelImages_HotelNotExist_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetHotelImagesByHotelIdQuery { HotelId = 999 };
            var validator = new GetHotelImagesByHotelIdValidator(_hotelRepositoryMock.Object);

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
        public async Task GetHotelImages_NegativeHotelId_ReturnsFailureResponse()
        {
            // Arrange
            var request = new GetHotelImagesByHotelIdQuery { HotelId = -1 };
            var validator = new GetHotelImagesByHotelIdValidator(_hotelRepositoryMock.Object);

            // Act
            var validationResult = await validator.ValidateAsync(request);
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, x => x.ErrorMessage == "Hotel Id greater than 0");
        }


        [Fact]
        public async Task GetHotelImages_ValidRequests_ReturnsSuccessResponseWithHotelImages()
        {
            // Arrange
            var request = new GetHotelImagesByHotelIdQuery { HotelId = 1 };
            var hotelImages = new List<Domain.Entities.HotelImage> { new Domain.Entities.HotelImage { HotelId = 1, ImageUrl = "image.jpg" } };
            var hotelImageDtos = new List<HotelImageDto> { new HotelImageDto { HotelId = 1, ImageUrl = "image.jpg" } };

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Hotel { Id = 1 });

            _repositoryMock.Setup(repo => repo.GetByHotelIdAsync(It.IsAny<int>()))
                .ReturnsAsync(hotelImages);

            _mapperMock.Setup(m => m.Map<ICollection<HotelImageDto>>(It.IsAny<IEnumerable<Domain.Entities.HotelImage>>()))
                .Returns(hotelImageDtos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(hotelImageDtos, response.HotelImages);
        }
    }
}