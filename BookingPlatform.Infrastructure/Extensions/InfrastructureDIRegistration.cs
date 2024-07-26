using BookingPlatform.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using BookingPlatform.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;
using BookingPlatform.Infrastructure.Services;


namespace BookingPlatform.Infrastructure.Extensions
{
    public static class InfrastructureDIRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookingPlatformDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.EnableRetryOnFailure()));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IAmenityRepository, AmenityRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFeaturedDealRepository, FeaturedDealRepository>();
            services.AddTransient<IHotelRepository, HotelRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IHotelImageRepository, HotelImageRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<BookingPlatformDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
