using BookingPlatform.Domain.Entities;
using BookingPlatform.Infrastructure.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.Extensions
{
    public static class StartupDbExtensions
    {
        public static async Task CreateDbIfNotExist(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<BookingPlatformDbContext>>();
            var bookingPlatformContext = services.GetRequiredService<BookingPlatformDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                var databaseCreate = bookingPlatformContext.Database.GetService<IDatabaseCreator>()
                    as RelationalDatabaseCreator;

                if (databaseCreate != null)
                {
                    logger.LogInformation("Enter databaseCreate");
                    if (!databaseCreate.CanConnect())
                    {
                        databaseCreate.Create();
                        logger.LogInformation("Enter databaseCreate Create");
                    }
                    if (!databaseCreate.HasTables())
                    {
                        databaseCreate.CreateTables();
                        logger.LogInformation("Enter databaseCreate CreateTables");
                    }

                    await DbInitializerSeedData.InitializeDatabase(bookingPlatformContext, userManager, roleManager);
                    logger.LogInformation("Enter databaseCreate InitializeDatabase");

                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
            }
        }
    }
}
