using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using RehearsalRoomBookingSystem.Common.Option;
using Microsoft.Extensions.Options;

namespace RehearsalRoomBookingSystem.Common.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _databasePath;
        private readonly string _connectionString;

        public DatabaseHelper(IOptions<DatabaseSettings> options)
        {
            _databasePath = options.Value.SqlitePath;
            _connectionString = options.Value.ConnectionString;
        }

        public void CreateSqlite()
        {
            if (!File.Exists(_databasePath))
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    //會員主表
                    var _sql = @"
                CREATE TABLE [Members] (
                   [MemberID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                   [Name] nvarchar(50) ,
                   [Phone] varchar(50) ,
                   [Card_Available_Hours] int,
                   [Memo] nvarchar(50),
                   [UpdateUser] nvarchar(50),
                   [UpdateDate] datetime
                ); ";

                    //會員時數交易紀錄表
                    _sql += @"
                CREATE TABLE [MemberTransactions] (
                   [MemberID] int,
                   [TransactionHours] int,
                   [CreateUser] nvarchar(50) COLLATE NOCASE,
                   [CreateDate] datetime
                ); ";

                    //測試資料
                    _sql += @"
INSERT INTO [Members] ([Name], [Phone], [Card_Available_Hours], [Memo], [UpdateUser], [UpdateDate])
VALUES
    ('John Doe', '123456789', 10, 'Test Memo 1', 'Admin', datetime('now')),
    ('Jane Smith', '987654321', 20, 'Test Memo 2', 'Admin', datetime('now')),
    ('Michael Johnson', '555555555', 15, 'Test Memo 3', 'Admin', datetime('now')),
    ('Emily Davis', '111111111', 5, 'Test Memo 4', 'Admin', datetime('now')),
    ('David Wilson', '999999999', 30, 'Test Memo 5', 'Admin', datetime('now')),
    ('Sarah Thompson', '777777777', 25, 'Test Memo 6', 'Admin', datetime('now')),
    ('Christopher Anderson', '222222222', 8, 'Test Memo 7', 'Admin', datetime('now')),
    ('Jessica Martinez', '888888888', 12, 'Test Memo 8', 'Admin', datetime('now')),
    ('Matthew Taylor', '444444444', 18, 'Test Memo 9', 'Admin', datetime('now')),
    ('Olivia Thomas', '666666666', 22, 'Test Memo 10', 'Admin', datetime('now')),
    ('Daniel Hernandez', '333333333', 7, 'Test Memo 11', 'Admin', datetime('now')),
    ('Sophia Moore', '555555555', 13, 'Test Memo 12', 'Admin', datetime('now')),
    ('Andrew Clark', '777777777', 9, 'Test Memo 13', 'Admin', datetime('now')),
    ('Isabella Lewis', '999999999', 16, 'Test Memo 14', 'Admin', datetime('now')),
    ('Joseph Young', '111111111', 21, 'Test Memo 15', 'Admin', datetime('now')),
    ('Ava Walker', '888888888', 6, 'Test Memo 16', 'Admin', datetime('now')),
    ('William Hall', '222222222', 11, 'Test Memo 17', 'Admin', datetime('now')),
    ('Mia Allen', '444444444', 17, 'Test Memo 18', 'Admin', datetime('now')),
    ('James Green', '666666666', 23, 'Test Memo 19', 'Admin', datetime('now')),
    ('Emma King', '333333333', 8, 'Test Memo 20', 'Admin', datetime('now'));
";

                    conn.Execute(_sql);
                }
            }
        }
    }
}
