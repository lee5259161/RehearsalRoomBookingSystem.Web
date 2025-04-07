using RehearsalRoomBookingSystem.Common.Helpers;
using RehearsalRoomBookingSystem.Common.Implement;
using RehearsalRoomBookingSystem.Common.Interface;
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
            // Add HttpContextAccessor
            services.AddHttpContextAccessor();

            // Common
            services.AddSingleton<DatabaseHelper>();
            services.AddSingleton<IEncryptHelper, EncryptHelper>();
            services.AddSingleton<IUserContextHelper, UserContextHelper>();

            // Repository
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IMemberTransactionsRepository, MemberTransactionsRepository>();

            // Service
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IMemberTransactionsService, MemberTransactionsService>();

            // MappingProfile
            services.AddScoped<IServiceMapProfile, ServiceMapProfile>();
            services.AddScoped<IControllerMapProfile, ControllerMapProfile>();

            return services;
        }
    }
}