using System;
using Autofac.Extensions.DependencyInjection;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // For seeding data into DB
            var host = CreateHostBuilder(args).Build();

            Log.Logger = new LoggerConfiguration()
                      .MinimumLevel.Debug()
                      .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                      .Enrich.FromLogContext()
                      .WriteTo.RollingFile("Logs//datingAppAPI-log-{Date}.log")
                      .WriteTo.Console()
                      .CreateLogger();

            // For seeding data into DB
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    Seed.SeedUsers(context);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occured during migration");
                }
            }

            try
            {
                Log.Information("Application Starting up");
                host.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
