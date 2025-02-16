using RehearsalRoomBookingSystem.Common.Helpers;
using RehearsalRoomBookingSystem.Common.Option;
using RehearsalRoomBookingSystem.Repository.Implements;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.Implements;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Web.Infrastructure.ServiceCollections;

namespace RehearsalRoomBookingSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 配置Options Pattern
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

            //Dependency Injection
            builder.Services.AddDependencyInjection();

            var app = builder.Build();

            //在專案第一次啟動的時候，檢查SQLite 檔案是否存在，如果不存在就建立SQLite 檔案
            var databaseHelper = app.Services.GetRequiredService<DatabaseHelper>();
            databaseHelper.CreateSqlite();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Member}/{action=Index}/{id?}");

            app.Run();
        }
    }
}