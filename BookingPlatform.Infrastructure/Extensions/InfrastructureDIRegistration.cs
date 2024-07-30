using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using BookingPlatform.Infrastructure.Repositories;
using BookingPlatform.Infrastructure.Services;
using BookingPlatform.Infrastructure.Services.Dtos;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BookingPlatform.Infrastructure.Extensions
{
    public static class InfrastructureDIRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookingPlatformDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.EnableRetryOnFailure()));

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IPdfService, PdfService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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
