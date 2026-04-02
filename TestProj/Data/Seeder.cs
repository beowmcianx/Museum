using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestProj.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TestProj.Data.Services;

namespace TestProj.Data
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

            // --- Create Roles ---
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

            // =========================
            // ADMIN USER
            // =========================
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

            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                var addRoleResult = await userManager.AddToRoleAsync(admin, Roles.Admin);
                if (!addRoleResult.Succeeded)
                {
                    logger.LogError("Failed to add admin user to role {Role}: {Errors}", Roles.Admin, string.Join(", ", addRoleResult.Errors));
                }
            }

            // =========================
            // CLIENT USER
            // =========================
            var clientEmail = "client@museum.com";
            var client = await userManager.FindByEmailAsync(clientEmail);

            if (client == null)
            {
                client = new UserModel
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    FullName = "Demo Client",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(client, "Client123!");
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed to create client user: {Errors}", string.Join(", ", createResult.Errors));
                }
                else
                {
                    logger.LogInformation("Client user {Email} created", clientEmail);
                }
            }

            if (!await userManager.IsInRoleAsync(client, Roles.Client))
            {
                var roleResult = await userManager.AddToRoleAsync(client, Roles.Client);
                if (!roleResult.Succeeded)
                {
                    logger.LogError("Failed to add client to role: {Errors}", string.Join(", ", roleResult.Errors));
                }
            }

            // =========================
            // WORKER USER
            // =========================
            var workerEmail = "worker@museum.com";
            var worker = await userManager.FindByEmailAsync(workerEmail);

            if (worker == null)
            {
                worker = new UserModel
                {
                    UserName = workerEmail,
                    Email = workerEmail,
                    FullName = "Demo Worker",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(worker, "Worker123!");
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed to create worker user: {Errors}", string.Join(", ", createResult.Errors));
                }
                else
                {
                    logger.LogInformation("Worker user {Email} created", workerEmail);
                }
            }

            if (!await userManager.IsInRoleAsync(worker, Roles.Worker))
            {
                var roleResult = await userManager.AddToRoleAsync(worker, Roles.Worker);
                if (!roleResult.Succeeded)
                {
                    logger.LogError("Failed to add worker to role: {Errors}", string.Join(", ", roleResult.Errors));
                }
            }

            // =========================
            // SEED TICKET TYPES
            // =========================
            try
            {
                var db = services.GetRequiredService<ApplicationDbContext>();

                if (await db.Museums.AnyAsync())
                {
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

        // --- Null Logger ---
        private class NullLogger : ILogger
        {
            public static readonly ILogger Instance = new NullLogger();

            public IDisposable BeginScope<TState>(TState state) => null!;

            public bool IsEnabled(LogLevel logLevel) => false;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                Exception exception, Func<TState, Exception, string> formatter)
            { }
        }
    }
}