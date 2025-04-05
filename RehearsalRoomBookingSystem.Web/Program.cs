using RehearsalRoomBookingSystem.Common.Helpers;
using RehearsalRoomBookingSystem.Common.Option;
using Microsoft.AspNetCore.Authentication.Cookies;
using RehearsalRoomBookingSystem.Web.Infrastructure.ServiceCollections;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.AspNetCore;

namespace RehearsalRoomBookingSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            // 設定認證機制
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromHours(12);
                    options.SlidingExpiration = true;
                });

            // 設定 Options Pattern
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

            // Dependency Injection
            builder.Services.AddDependencyInjection();

            var app = builder.Build();

            // 在系統第一次執行的時候，檢查SQLite 檔案是否存在，如果不存在就建立SQLite 檔案
            var databaseHelper = app.Services.GetRequiredService<DatabaseHelper>();
            databaseHelper.CreateSqlite();
            // 檢查SQLite 是否需更新
            databaseHelper.MigrateToNextVersion();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            // Create Logs directory if it doesn't exist
            var logDir = Path.Combine(app.Environment.ContentRootPath, "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            app.UseRouting();

            // 加入驗證中介軟體
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Member}/{action=Index}/{id?}");

            try
            {
                Log.Information("Starting web application");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}