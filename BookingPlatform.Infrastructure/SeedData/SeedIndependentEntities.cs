using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.SeedData
{
    public static class SeedIndependentEntities
    {
        public static void InitializeAmenities(BookingPlatformDbContext dbContext)
        {
            if (dbContext.Amenities.Any()) return;

            var amenities = new Amenity[]
            {
                new Amenity {Name = "Free Wi-Fi"},
                new Amenity {Name = "Swimming Pool"},
                new Amenity {Name = "Gym"},
                new Amenity {Name = "Spa"},
                new Amenity {Name = "Restaurant"},
                new Amenity {Name = "Bar"},
                new Amenity {Name = "Parking"},
            };

            dbContext.Amenities.AddRange(amenities);
            dbContext.SaveChanges();
        }

        public static void InitializeCities(BookingPlatformDbContext dbContext)
        {
            if (dbContext.Cities.Any()) return;

            var cities = new City[]
            {
                new City {Name = "Buenos Aires", ThumbnailUrl = "someurl.com", Country = "Argentina", PostalCode = "B1228"},
                new City {Name = "Libertador San Martín", ThumbnailUrl = "someurl.com", Country = "Argentina", PostalCode = "3103"},
                new City {Name = "Rosario", ThumbnailUrl = "someurl.com", Country = "Argentina", PostalCode = "S2000"},
                new City {Name = "Bogotá", ThumbnailUrl = "someurl.com", Country = "Colombia", PostalCode = "110110"},
                new City {Name = "Neiva", ThumbnailUrl = "someurl.com", Country = "Colombia", PostalCode = "410001"},
                new City {Name = "New York", ThumbnailUrl = "someurl.com", Country = "United States", PostalCode = "10001"},
                new City {Name = "Los Angeles", ThumbnailUrl = "someurl.com", Country = "United States", PostalCode = "90001"},
            };

            dbContext.Cities.AddRangeAsync(cities);
            dbContext.SaveChanges();
        }

        public static void InitializeRoomTypes(BookingPlatformDbContext dbContext)
        {
            if (dbContext.RoomTypes.Any()) return;

            var roomTypes = new RoomType[]
            {
                new RoomType {Name = "Luxury"},
                new RoomType {Name = "Budget"},
                new RoomType {Name = "Boutique"}
            };

            dbContext.RoomTypes.AddRange(roomTypes);
            dbContext.SaveChanges();
        }

        public static async Task<List<string>> InitializeUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleEntity = new Role();
                    roleEntity.Name = role;
                    await roleManager.CreateAsync(roleEntity);
                }
            }

            List<string> usersCreated = new List<string>();

            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    usersCreated.Add(adminUser.Id);
                }
            }

            if (await userManager.FindByEmailAsync("customer@example.com") == null)
            {
                var customerUser = new User
                {
                    FirstName = "Customer",
                    LastName = "User",
                    UserName = "customer@example.com",
                    Email = "customer@example.com",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(customerUser, "Customer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                    usersCreated.Add(customerUser.Id);
                }
            }

            if (await userManager.FindByEmailAsync("maria.romero@gmail.com") == null)
            {
                var myUser = new User
                {
                    FirstName = "Maria",
                    LastName = "Romero",
                    UserName = "maria.romero@gmail.com",
                    Email = "maria.romero@gmail.com",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1988, 12, 12),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(myUser, "MyUser@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(myUser, "Admin");
                    usersCreated.Add(myUser.Id);
                }
            }

            return usersCreated;
        }
    }
}
