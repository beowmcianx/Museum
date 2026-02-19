using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestProj.Models;
using TestProj.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TestProj.Models
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetRequiredService<UserManager<UserModel>>();
            var loggerFactory = services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var logger = loggerFactory?.CreateLogger(typeof(IdentitySeeder));

            logger ??= NullLogger.Instance;

            string[] roles = { Roles.Client, Roles.Worker, Roles.Admin };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var createRoleResult = await roleManager.CreateAsync(new IdentityRole<int>(role));
                    if (!createRoleResult.Succeeded)
                    {
                        logger.LogError("Failed to create role {Role}: {Errors}", role, string.Join(", ", createRoleResult.Errors));
                    }
                    else
                    {
                        logger.LogInformation("Created role {Role}", role);
                    }
                }
                else
                {
                    logger.LogInformation("Role {Role} already exists", role);
                }
            }

            var adminEmail = "admin@museum.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new UserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(admin, "Admin123!");
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", createResult.Errors));
                    return;
                }

                logger.LogInformation("Admin user {Email} created", adminEmail);
            }
            else
            {
                logger.LogInformation("Admin user {Email} already exists (Id={Id})", adminEmail, admin.Id);
            }

            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                var addRoleResult = await userManager.AddToRoleAsync(admin, Roles.Admin);
                if (!addRoleResult.Succeeded)
                {
                    logger.LogError("Failed to add admin user {Email} to role {Role}: {Errors}", adminEmail, Roles.Admin, string.Join(", ", addRoleResult.Errors));
                }
                else
                {
                    logger.LogInformation("Admin user {Email} added to role {Role}", adminEmail, Roles.Admin);
                }
            }
            else
            {
                logger.LogInformation("Admin user {Email} already in role {Role}", adminEmail, Roles.Admin);
            }

            // --- Seed ticket types for seeded museums ---
            try
            {
                var db = services.GetRequiredService<ApplicationDbContext>();

                // Ensure DB is available
                if (await db.Museums.AnyAsync())
                {
                    // Predefined ticket types to seed for each museum (if museum has none)
                    var defaultTicketTypes = new List<(string Name, decimal Price)>
                    {
                        ("Adult", 15.00m),
                        ("Child", 8.00m),
                        ("Senior", 12.00m)
                    };

                    var museumIds = await db.Museums.Select(m => m.MuseumId).ToListAsync();

                    foreach (var mid in museumIds)
                    {
                        var exists = await db.TicketTypes.AnyAsync(tt => tt.MuseumId == mid);
                        if (!exists)
                        {
                            var toAdd = defaultTicketTypes.Select(t => new TicketTypeModel
                            {
                                Name = t.Name,
                                Price = t.Price,
                                IsActive = true,
                                MuseumId = mid
                            }).ToList();

                            await db.TicketTypes.AddRangeAsync(toAdd);
                        }
                    }

                    await db.SaveChangesAsync();
                    logger.LogInformation("Seeded default ticket types for museums.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed seeding ticket types.");
            }
        }

        // Null logger helper to avoid adding package references
        private class NullLogger : ILogger
        {
            public static readonly ILogger Instance = new NullLogger();
            public IDisposable BeginScope<TState>(TState state) => null!;
            public bool IsEnabled(LogLevel logLevel) => false;
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
        }
    }
}
