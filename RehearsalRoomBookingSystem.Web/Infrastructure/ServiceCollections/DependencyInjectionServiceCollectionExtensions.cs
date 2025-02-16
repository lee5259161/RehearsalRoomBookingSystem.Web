using RehearsalRoomBookingSystem.Common.Helpers;
using RehearsalRoomBookingSystem.Repository.Implements;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.Implements;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Service.MappingProfile;
using RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile;

namespace RehearsalRoomBookingSystem.Web.Infrastructure.ServiceCollections
{
    /// <summary>
    /// DI注入在Program.cs的Service的註冊設定
    /// </summary>
    public static class DependencyInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// DI注入註冊設定
        /// </summary>
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Common
            services.AddSingleton<DatabaseHelper>();

            // Repository
            services.AddScoped<IMemberRepository, MemberRepository>();

            // Service
            services.AddScoped<IMemberService, MemberService>();

            // MappingProfile
            services.AddScoped<IServiceMapProfile, ServiceMapProfile>();
            services.AddScoped<IControllerMapProfile, ControllerMapProfile>();

            return services;
        }
    }
}