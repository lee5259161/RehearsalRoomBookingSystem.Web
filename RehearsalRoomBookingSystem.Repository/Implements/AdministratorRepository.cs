using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Dapper;
using RehearsalRoomBookingSystem.Common.Option;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Common.Interface;
using RehearsalRoomBookingSystem.Common.Implement;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RehearsalRoomBookingSystem.Repository.Implements
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly string _connectionString;
        private readonly IEncryptHelper _encryptHelper;
        private const string ENCRYPT_SALT = "RoomBooking2025";

        public AdministratorRepository(
            IOptions<DatabaseSettings> options, 
            IEncryptHelper encryptHelper)
        {
            _connectionString = options.Value.ConnectionString;
            _encryptHelper = encryptHelper;
        }

        public AdministratorEntity ValidateLogin(string account, string password)
        {
            var encryptedPassword = _encryptHelper.SHAEncrypt(password, ENCRYPT_SALT);

            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT [AdminID], [TypeID], [Name], [Account], 
                                       [Password], [UpdateUser], [UpdateDate]
                                FROM [Administrators] 
                                WHERE [Account] = @Account 
                                AND [Password] = @Password";

                var admin = connection.QueryFirstOrDefault<AdministratorEntity>(
                    query,
                    new { Account = account, Password = encryptedPassword }
                );

                return admin;
            }
        }

        public AdministratorEntity GetByAccount(string account)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT [AdminID], [TypeID], [Name], [Account], 
                                       [Password], [UpdateUser], [UpdateDate]
                                FROM [Administrators] 
                                WHERE [Account] = @Account";

                var admin = connection.QueryFirstOrDefault<AdministratorEntity>(
                    query,
                    new { Account = account }
                );

                return admin;
            }
        }
    }
}