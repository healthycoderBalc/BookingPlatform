using BookingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.SeedData
{
    public static class SeedRoomRelatedEntities
    {
        public static void InitializeRooms(BookingPlatformDbContext dbContext)
        {
            if (dbContext.Rooms.Any()) return;

            var rooms = new Room[]
            {
                new Room
                {
                    HotelId = 1,
                    RoomNumber = "101",
                    TypeId = 1,
                    Description = "A budget single room.",
                    PricePerNight = 100,
                    AdultCapacity = 1,
                    ChildrenCapacity = 0,
                    IsOperational = true
                },
                new Room
                {
                    HotelId = 1,
                    RoomNumber = "102",
                    TypeId = 1,
                    Description = "A luxury double room.",
                    PricePerNight = 250,
                    AdultCapacity = 2,
                    ChildrenCapacity = 1,
                    IsOperational = true
                },
                new Room
                {
                    HotelId = 2,
                    RoomNumber = "101",
                    TypeId = 3,
                    Description = "A boutique family room",
                    PricePerNight = 200,
                    AdultCapacity = 2,
                    ChildrenCapacity = 2,
                    IsOperational = true
                }
            };

            dbContext.Rooms.AddRange(rooms);
            dbContext.SaveChanges();
        }

        public static void InitializeBookings(BookingPlatformDbContext dbContext, List<string> userIds)
        {
            if (dbContext.Bookings.Any()) return;

            var bookings = new Booking[]
            {
                new Booking
                {
                    UserId = userIds[0],
                    RoomId = 1,
                    CheckInDate = new DateTime(2024, 7, 20),
                    CheckOutDate = new DateTime(2024, 7, 25),
                    TotalPrice = 500,
                    SpecialRequests = "None",
                    ConfirmationNumber = "ABC123"
                },
                new Booking
                {
                    UserId =userIds[1],
                    RoomId = 2,
                    CheckInDate = new DateTime(2024, 8, 15),
                    CheckOutDate = new DateTime(2024, 8, 20),
                    TotalPrice = 750,
                    SpecialRequests = "Late check-out",
                    ConfirmationNumber = "DEF456"
                }
            };

            dbContext.Bookings.AddRange(bookings);
            dbContext.SaveChanges();
        }

        public static void InitializePayments(BookingPlatformDbContext dbContext)
        {
            if (dbContext.Payments.Any()) return;

            var payments = new Payment[]
            {
                new Payment
                {
                    BookingId = 1,
                    PaymentDate = DateTime.UtcNow,
                    Amount = 500,
                    PaymentMethod = "Credit Card",
                    PaymentStatus = "Paid"
                },
                new Payment
                {
                    BookingId = 2,
                    PaymentDate = DateTime.UtcNow,
                    Amount = 750,
                    PaymentMethod = "Credit Card",
                    PaymentStatus = "Paid"
                }
            };

            dbContext.Payments.AddRange(payments);
            dbContext.SaveChanges();
        }


        public static void InitializeRoomImages(BookingPlatformDbContext dbContext)
        {
            if (dbContext.RoomImages.Any()) return;

            var roomImages = new RoomImage[]
            {
                new RoomImage
                {
                    RoomId = 1,
                    ImageUrl = "room1_image1_url.com"
                },
                new RoomImage
                {
                    RoomId = 2,
                    ImageUrl = "room2_image1_url.com"
                }
            };

            dbContext.RoomImages.AddRange(roomImages);
            dbContext.SaveChanges();
        }

        public static void InitializeFeaturedDeals(BookingPlatformDbContext dbContext)
        {
            if (dbContext.FeaturedDeals.Any()) return;

            var featuredDeals = new FeaturedDeal[]
            {
                new FeaturedDeal
                {
                    RoomId = 1,
                    DealActive = true,
                    DiscountedPricePerNight = 80
                },
                new FeaturedDeal
                {
                    RoomId = 2,
                    DealActive = true,
                    DiscountedPricePerNight = 120
                }
            };

            dbContext.FeaturedDeals.AddRange(featuredDeals);
            dbContext.SaveChanges();
        }
    }
}
