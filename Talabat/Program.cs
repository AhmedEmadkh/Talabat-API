using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
			// Configure Identity Connection
			builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
			{
				Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});
			// Configure Redis Connection
			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            #endregion

            var app = builder.Build();

            #region Update-Database [Automatic]
            using var Scope = app.Services.CreateScope(); // Group of services lifetime scooped
            var Services = Scope.ServiceProvider; // Services Itself

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>(); // Create Logger Factory for logging the Exceptions

            try
            {
                var DbContext = Services.GetRequiredService<StoreContext>(); // Ask CLR fro createing object fro DbContext Explicitly
                await DbContext.Database.MigrateAsync(); // Update-Database for each Migration Automatic

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>(); // Ask CLR fro createing object fro DbContext Explicitly
				await IdentityDbContext.Database.MigrateAsync(); // Update-Database for each Migration Automatic

                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsyn(UserManager);
				await StoreContextSeed.SeedAsync(DbContext);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();

                Logger.LogError(ex, "An Error Occured During Applying The Migrations");
                
            }
            #endregion

            #region Configure - Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UserSwaggerMiddlewares();
            }

            // Configure Not Found Controller
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
