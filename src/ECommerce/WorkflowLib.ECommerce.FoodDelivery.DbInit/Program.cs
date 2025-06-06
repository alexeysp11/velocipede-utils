﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WorkflowLib.ECommerce.FoodDelivery.DbInit.Models;
using WorkflowLib.ECommerce.FoodDelivery.Core.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace WorkflowLib.ECommerce.FoodDelivery.DbInit;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var app = host.Services.GetRequiredService<IStartupInstance>();
        app.Start();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                ConfigureServices(services);
            });

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register application instance.
        services.AddSingleton<IStartupInstance, StartupInstance>();

        // Configuration settings.
        var appsettingsConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var appsettings = appsettingsConfig.GetSection("DbInitSettings").Get<DbInitSettings>();
        if (string.IsNullOrEmpty(appsettings.ConnectionString))
        {
            throw new Exception($"Cannot start the application: connection string is not initialized");
        }
        services.AddSingleton(appsettings);
        services.AddDbContext<FoodDeliveryDbContext>(options => options.UseNpgsql(appsettings.ConnectionString));
    }
}
