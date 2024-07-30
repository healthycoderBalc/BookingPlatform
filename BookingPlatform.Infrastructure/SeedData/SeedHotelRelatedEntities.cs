using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.SeedData
{
    public static class SeedHotelRelatedEntities
    {
        public static void InitializeHotels(BookingPlatformDbContext dbContext)
        {
            if (dbContext.Hotels.Any()) return;

            var hotels = new Hotel[]
            {
                new Hotel
                {
                    Name = "Hotel Buenos Aires",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Buenos Aires",
                    LongDescription = "A very nice hotel in Buenos Aires with many amenities.",
                    Address = "Street 123",
                    CityId = 1,
                    StarRating = 4,
                    Latitude = -34.603722M,
                    Longitude = -58.381592M,
                    OwnerName = "John Doe"
                },
                new Hotel
                {
                    Name = "Hotel Libertador",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Libertador San Martín",
                    LongDescription = "A very nice hotel in Libertador San Martín with many amenities.",
                    Address = "Street 456",
                    CityId = 2,
                    StarRating = 3,
                    Latitude = -32.072028M,
                    Longitude = -60.647345M,
                    OwnerName = "Jane Smith"
                },
                new Hotel
                {
                    Name = "Hotel Jalisco",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice Mexican hotel in Libertador San Martín",
                    LongDescription = "A very nice Mexican hotel in Libertador San Martín with many amenities.",
                    Address = "Uruguay 128",
                    CityId = 2,
                    StarRating = 3,
                    Latitude = -33.072028M,
                    Longitude = -61.647345M,
                    OwnerName = "Luis Freise"
                },
                new Hotel
                {
                    Name = "Hotel Rosario",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Rosario",
                    LongDescription = "A very nice hotel in Rosario with many amenities.",
                    Address = "Street 123",
                    CityId = 3,
                    StarRating = 4,
                    Latitude = -43.072028M,
                    Longitude = -71.647345M,
                    OwnerName = "Sofía Vergara"
                },
                new Hotel
                {
                    Name = "Hotel Bogotá",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Bogotá",
                    LongDescription = "A very nice hotel in Bogotá with many amenities.",
                    Address = "Street 123",
                    CityId = 4,
                    StarRating = 4,
                    Latitude = -43.072028M,
                    Longitude = -71.647345M,
                    OwnerName = "Sofía Vergara"
                },
                new Hotel
                {
                    Name = "Hotel Neiva",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Neiva",
                    LongDescription = "A very nice hotel in Neiva with many amenities.",
                    Address = "Street 345",
                    CityId = 5,
                    StarRating = 4,
                    Latitude = -43.072028M,
                    Longitude = -71.647345M,
                    OwnerName = "Sofía Vergara"
                },
                new Hotel
                {
                    Name = "Hotel New York",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in New York",
                    LongDescription = "A very nice hotel in New York with many amenities.",
                    Address = "Street 456",
                    CityId = 6,
                    StarRating = 5,
                    Latitude = -43.072028M,
                    Longitude = -71.647345M,
                    OwnerName = "Cristian Castro"
                },
                new Hotel
                {
                    Name = "Hotel Los Angeles",
                    ThumbnailUrl = "someurl.com",
                    ShortDescription = "A nice hotel in Los Angeles",
                    LongDescription = "A very nice hotel in Los Angeles with many amenities.",
                    Address = "Street 987",
                    CityId = 7,
                    StarRating = 2,
                    Latitude = -43.072028M,
                    Longitude = -71.647345M,
                    OwnerName = "Peter Parker"
                },
            };

            dbContext.Hotels.AddRange(hotels);
            dbContext.SaveChanges();
        }

        public static void InitializeHotelReviews(BookingPlatformDbContext dbContext, List<string> userIds)
        {
            if (dbContext.HotelReviews.Any()) return;

            var reviews = new HotelReview[]
            {
                new HotelReview
                {
                    UserId = userIds[0],
                    HotelId = 1,
                    ReviewText = "Great hotel with excellent service!"
                },
                new HotelReview
                {
                    UserId = userIds[1],
                    HotelId = 2,
                    ReviewText = "Nice location, but the rooms were a bit small."
                }
            };

            dbContext.HotelReviews.AddRange(reviews);
            dbContext.SaveChanges();
        }

        public static void InitializeHotelImages(BookingPlatformDbContext dbContext)
        {
            if (dbContext.HotelImages.Any()) return;

            var images = new HotelImage[]
            {
                new HotelImage
                {
                    HotelId = 1,
                    ImageUrl = "image1_url.com"
                },
                new HotelImage
                {
                    HotelId = 1,
                    ImageUrl = "image12_url.com"
                },
                new HotelImage
                {
                    HotelId = 2,
                    ImageUrl = "image2_url.com"
                }
            };

            dbContext.HotelImages.AddRange(images);
            dbContext.SaveChanges();
        }

        public static void InitializeHotelAmenities(BookingPlatformDbContext dbContext)
        {
            if (dbContext.HotelAmenities.Any()) return;

            var hotelAmenities = new HotelAmenity[]
            {
                new HotelAmenity
                {
                    HotelId = 1,
                    AmenityId = 1
                },
                new HotelAmenity
                {
                    HotelId = 1,
                    AmenityId = 2
                },
                new HotelAmenity
                {
                    HotelId = 2,
                    AmenityId = 1
                },
                new HotelAmenity
                {
                    HotelId = 2,
                    AmenityId = 3
                }
            };

            dbContext.HotelAmenities.AddRange(hotelAmenities);
            dbContext.SaveChanges();
        }

    }
}
