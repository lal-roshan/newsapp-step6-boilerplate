﻿using AuthenticationService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NewsService.Models;
using ReminderService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UserService.Models;
using Xunit;

namespace Test.ControllerTests.IntegrationTest
{
    public class NewsWebApplicationFactory<TStartup> : WebApplicationFactory<NewsService.Startup>
    {
        NewsContext newsDb = null;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings-integration.json");

                var config = new ConfigurationBuilder().AddJsonFile(configPath).Build();
                services.AddSingleton<IConfiguration>(config);
                services.AddScoped<NewsContext>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    newsDb = scopedServices.GetRequiredService<NewsContext>();

                    var logger = scopedServices.GetRequiredService<ILogger<NewsWebApplicationFactory<TStartup>>>();

                    try
                    {
                        // Seed the database with some specific test data.
                        newsDb.News.DeleteMany(Builders<UserNews>.Filter.Empty);
                        newsDb.News.InsertMany(new List<UserNews> {
                            new UserNews{ UserId="Jack", NewsList= new List<News>{{
                                    new News{
                                        NewsId=101,
                                        Title = "IT industry in 2020",
                                        Content = "It is expected to have positive growth in 2020.",
                                        PublishedAt = DateTime.Now, UrlToImage = null,Url=null
                                    }
                                }
                              }
                            }

                        });
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
        protected override void Dispose(bool disposing)
        {
            newsDb.News.DeleteMany(Builders<UserNews>.Filter.Empty);
            newsDb = null;
        }
    }
    public class UserWebApplicationFactory<TStartup> : WebApplicationFactory<UserService.Startup>
    {
        UserContext userDb = null;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings-integration.json");

                var config = new ConfigurationBuilder().AddJsonFile(configPath).Build();
                services.AddSingleton<IConfiguration>(config);
                services.AddScoped<UserContext>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    userDb = scopedServices.GetRequiredService<UserContext>();

                    var logger = scopedServices.GetRequiredService<ILogger<UserWebApplicationFactory<TStartup>>>();

                    try
                    {
                        // Seed the database with some specific test data.
                        userDb.Users.DeleteMany(Builders<UserProfile>.Filter.Empty);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
        protected override void Dispose(bool disposing)
        {
            userDb.Users.DeleteMany(Builders<UserProfile>.Filter.Empty);
            userDb = null;
        }
    }
    public class ReminderWebApplicationFactory<TStartup> : WebApplicationFactory<ReminderService.Startup>
    {
        ReminderContext reminderDb = null;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings-integration.json");

                var config = new ConfigurationBuilder().AddJsonFile(configPath).Build();
                services.AddSingleton<IConfiguration>(config);
                services.AddScoped<ReminderContext>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    reminderDb = scopedServices.GetRequiredService<ReminderContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<ReminderWebApplicationFactory<TStartup>>>();

                    try
                    {
                        // Seed the database with some specific test data.
                        reminderDb.Reminders.DeleteMany(Builders<ReminderService.Models.Reminder>.Filter.Empty);
                        reminderDb.Reminders.InsertMany(new List<ReminderService.Models.Reminder> {
                           new ReminderService.Models.Reminder
                           {
                               UserId="Jack",
                               Email="jack@ymail.com",
                               NewsReminders= new List<ReminderSchedule>
                               {
                                   new ReminderSchedule
                                   {
                                       NewsId=101,
                                       Schedule=DateTime.Now.AddDays(2)
                                   }
                               }
                           }
                        });
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
        protected override void Dispose(bool disposing)
        {
            reminderDb.Reminders.DeleteMany(Builders<ReminderService.Models.Reminder>.Filter.Empty);
            reminderDb = null;
        }
    }

    public class AuthWebApplicationFactory<TStartup> : WebApplicationFactory<AuthenticationService.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a database context (KeepNoteContext) using an in-memory database for testing.
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryAuthDB");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var authDb = scopedServices.GetRequiredService<AuthDbContext>();

                    var logger = scopedServices.GetRequiredService<ILogger<AuthWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    authDb.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with some specific test data.
                        authDb.Users.Add(new AuthenticationService.Models.User { UserId = "Jack", Password = "password@123" });
                        authDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }

    [CollectionDefinition("Auth API")]
    public class DbCollection : ICollectionFixture<AuthWebApplicationFactory<AuthenticationService.Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
