using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.SeedData
{
    public static class DbInitializerSeedData
    {
        public static async Task InitializeDatabase(BookingPlatformDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            SeedIndependentEntities.InitializeAmenities(dbContext);
            SeedIndependentEntities.InitializeCities(dbContext);
            SeedIndependentEntities.InitializeRoomTypes(dbContext);
            List<string> userIds = await SeedIndependentEntities.InitializeUsers(userManager, roleManager);
            SeedHotelRelatedEntities.InitializeHotels(dbContext);
            SeedRoomRelatedEntities.InitializeRooms(dbContext);
            SeedRoomRelatedEntities.InitializeBookings(dbContext, userIds);
            SeedRoomRelatedEntities.InitializePayments(dbContext);
            SeedHotelRelatedEntities.InitializeHotelReviews(dbContext, userIds);
            SeedHotelRelatedEntities.InitializeHotelImages(dbContext);
            SeedHotelRelatedEntities.InitializeHotelAmenities(dbContext);
            SeedRoomRelatedEntities.InitializeRoomImages(dbContext);
            SeedRoomRelatedEntities.InitializeFeaturedDeals(dbContext);
        }
    }

}

