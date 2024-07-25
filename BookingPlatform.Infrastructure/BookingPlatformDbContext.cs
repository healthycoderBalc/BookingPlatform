using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure
{
    public class BookingPlatformDbContext : IdentityDbContext<User, Role, string>
    {
        public BookingPlatformDbContext(DbContextOptions<BookingPlatformDbContext> options) : base(options) { }

        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<HotelReview> HotelReviews { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<FeaturedDeal> FeaturedDeals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Booking>()
               .Property(mi => mi.TotalPrice)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<FeaturedDeal>()
               .Property(mi => mi.DiscountedPricePerNight)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Hotel>()
               .Property(mi => mi.Latitude)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Hotel>()
               .Property(mi => mi.Longitude)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
               .Property(mi => mi.Amount)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
               .Property(mi => mi.PricePerNight)
               .HasColumnType("decimal(18,2)");

        }

    }
}
