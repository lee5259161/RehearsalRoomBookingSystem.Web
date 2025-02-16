//namespace RehearsalRoomBookingSystem.Web.Infrastructure.ServiceCollections
//{

//    /// <summary>
//    /// DB連線在Program.cs的Service設定
//    /// </summary>
//    public static class DatabaseConnectionServiceCollectionExtensions
//    {
//        /// <summary>Adds the database connection.</summary>
//        /// <param name="services">The services.</param>
//        /// <returns></returns>
//        public static IServiceCollection AddDatabaseConnection(this IServiceCollection services)
//        {
//            services.AddOptions<DataBaseConnectionOptions>()
//                    //.Configure<IEvertrustDatabases>
//                    (
//                        (options, databases) =>
//                        {
//                            var connectionString = databases.GetConnectionString("DefaultConnection");
//                            options.DefaultConnection = connectionString;
//                        }
//                    );

//            return services;
//        }
//    }
//}