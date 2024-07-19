using BookingPlatform.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.Extensions
{
    public static class InfrastructureDIRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookingPlatformDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.EnableRetryOnFailure()));

            services.AddScoped(typeof(IRepository<>), typeof(IRepository<>));
            return services;
        }
    }
}
