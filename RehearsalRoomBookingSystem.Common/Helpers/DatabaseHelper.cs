using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using RehearsalRoomBookingSystem.Common.Option;
using Microsoft.Extensions.Options;
using RehearsalRoomBookingSystem.Common.Interface;
using RehearsalRoomBookingSystem.Common.Implement;

namespace RehearsalRoomBookingSystem.Common.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _databasePath;
        private readonly string _connectionString;
        private readonly int _SQLiteVersion = 2;
        private readonly IEncryptHelper _encryptHelper;
        private const string ENCRYPT_SALT = "RoomBooking2025";

        public DatabaseHelper(IOptions<DatabaseSettings> options, IEncryptHelper encryptHelper)
        {
            _databasePath = options.Value.SqlitePath;
            _connectionString = options.Value.ConnectionString;
            _encryptHelper = encryptHelper;
        }

        public void CreateSqlite()
        {
            if (!File.Exists(_databasePath))
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    // 建立版本控制表
                    var sql = @"
                    CREATE TABLE [DatabaseVersion] (
                        [Version] INTEGER NOT NULL,
                        [UpdateDate] datetime NOT NULL DEFAULT (datetime('now'))
                    );";

                    // 原有的資料表建立
                    sql += @"
                    CREATE TABLE [Members] (
                       [MemberId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                       [Name] TEXT,
                       [Phone] TEXT UNIQUE,
                       [Card_Available_Hours] int,
                       [Memo] TEXT,
                       [UpdateUser] TEXT NOT NULL,
                       [UpdateDate] datetime
                    );";

                    //新增一個交易類型的參考表
                    sql += @"
                    CREATE TABLE [TransactionTypes] (
                        [TypeId] int NOT NULL PRIMARY KEY,
                        [TypeName] TEXT NOT NULL,
                        [Description] TEXT NOT NULL
                    );

                    -- 插入交易類型定義
                    INSERT INTO [TransactionTypes] ([TypeId], [TypeName], [Description])
                    VALUES 
                        (1, '購買時數', '會員購買預付時數'),
                        (2, '使用時數', '會員使用預付時數'),
                        (3, '回復購買', '回復會員購買時數紀錄'),
                        (4, '回復使用', '回復會員使用時數紀錄');";

                    sql += @"
                    -- 主要的交易記錄表
                    CREATE TABLE [MemberTransactions] (
                    [TransactionId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [MemberId] int NOT NULL,
                    [TypeId] int NOT NULL,
                    [TransactionHours] int NOT NULL,
                    [CreateUser] TEXT NOT NULL,
                    [CreateDate] datetime NOT NULL DEFAULT (datetime('now')),
                    [RecoveryTransactionId] int NULL, -- 用於追蹤被回復的原始交易
                    [IsRecovered] boolean NOT NULL DEFAULT 0, -- 標記該筆交易是否已被回復
                    [RecoverUser] TEXT NULL, -- 新增：執行回復的使用者
                    [RecoverDate] datetime NULL, -- 新增：執行回復的時間
                    FOREIGN KEY ([MemberId]) REFERENCES [Members]([MemberId]),  -- 外鍵約束
                    FOREIGN KEY ([TypeId]) REFERENCES [TransactionTypes]([TypeId]), -- 外鍵約束
                    FOREIGN KEY ([RecoveryTransactionId]) REFERENCES [MemberTransactions]([TransactionId]), -- 外鍵參考原始交易
                    CHECK ([TransactionHours] != 0) -- 確保交易時數不為零
                    );";

                    sql += @"
                        CREATE INDEX [IX_MemberTransactions_MemberId] ON [MemberTransactions]([MemberId]);
                        CREATE INDEX [IX_MemberTransactions_Type] ON [MemberTransactions]([TypeId]);
                        CREATE INDEX [IX_MemberTransactions_CreateDate] ON [MemberTransactions]([CreateDate]);";

                    // 插入測試資料
                    sql += @"
INSERT INTO [Members] ([Name], [Phone], [Card_Available_Hours], [Memo], [UpdateUser], [UpdateDate])
VALUES
    ('John Doe', '0912345678', 10, 'Test Memo 1', 'Admin', datetime('now')),
    ('Jane Smith', '0923456789', 20, 'Test Memo 2', 'Admin', datetime('now')),
    ('Michael Johnson', '0934567890', 15, 'Test Memo 3', 'Admin', datetime('now')),
    ('Emily Davis', '0945678901', 5, 'Test Memo 4', 'Admin', datetime('now')),
    ('David Wilson', '0956789012', 30, 'Test Memo 5', 'Admin', datetime('now')),
    ('Sarah Thompson', '0967890123', 25, 'Test Memo 6', 'Admin', datetime('now')),
    ('Christopher Anderson', '0978901234', 8, 'Test Memo 7', 'Admin', datetime('now')),
    ('Jessica Martinez', '0989012345', 12, 'Test Memo 8', 'Admin', datetime('now')),
    ('Matthew Taylor', '0901123456', 18, 'Test Memo 9', 'Admin', datetime('now')),
    ('Olivia Thomas', '0913234567', 22, 'Test Memo 10', 'Admin', datetime('now')),
    ('Daniel Hernandez', '0924345678', 7, 'Test Memo 11', 'Admin', datetime('now')),
    ('Sophia Moore', '0935456789', 13, 'Test Memo 12', 'Admin', datetime('now')),
    ('Andrew Clark', '0946567890', 9, 'Test Memo 13', 'Admin', datetime('now')),
    ('Isabella Lewis', '0957678901', 16, 'Test Memo 14', 'Admin', datetime('now')),
    ('Joseph Young', '0968789012', 21, 'Test Memo 15', 'Admin', datetime('now')),
    ('Ava Walker', '0979890123', 6, 'Test Memo 16', 'Admin', datetime('now')),
    ('William Hall', '0980901234', 11, 'Test Memo 17', 'Admin', datetime('now')),
    ('Mia Allen', '0902012345', 17, 'Test Memo 18', 'Admin', datetime('now')),
    ('James Green', '0914123456', 23, 'Test Memo 19', 'Admin', datetime('now')),
    ('Emma King', '0925234567', 8, 'Test Memo 20', 'Admin', datetime('now'));
";

                    // 設定初始版本號為 1
                    sql += @"
                    INSERT INTO [DatabaseVersion] ([Version], [UpdateDate])
                    VALUES (1, datetime('now'));";

                    conn.Execute(sql);
                }
            }
        }

        public void MigrateToNextVersion()
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 檢查當前版本
                        var currentVersion = conn.QuerySingle<int>("SELECT Version FROM DatabaseVersion");
                        if (currentVersion != _SQLiteVersion)
                        {
                            // 修改 Members 表的 Memo 欄位
                            conn.Execute(@"
                                -- 創建臨時表
                                CREATE TABLE [Members_Temp] (
                                   [MemberId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                   [Name] TEXT,
                                   [Phone] TEXT,
                                   [Card_Available_Hours] int,
                                   [Memo] TEXT,
                                   [UpdateUser] TEXT,
                                   [UpdateDate] datetime DEFAULT (datetime('now'))
                                );

                                -- 複製資料到臨時表
                                INSERT INTO [Members_Temp]
                                SELECT * FROM [Members];

                                -- 刪除原表
                                DROP TABLE [Members];

                                -- 重命名臨時表
                                ALTER TABLE [Members_Temp] RENAME TO [Members];
                            ");

                            // 檢查是否已經存在 Administrators 表格
                            var tableExists = conn.QueryFirstOrDefault<int>(
                                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Administrators'"
                            );

                            if (tableExists == 0)
                            {
                                // 建立管理人員資料表
                                conn.Execute(@"
                                    CREATE TABLE [Administrators] (
                                    [AdminId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                    [TypeId] int NOT NULL,
                                    [Name] TEXT,
                                    [Account] TEXT NOT NULL UNIQUE,
                                    [Password] TEXT NOT NULL,
                                    [UpdateUser] TEXT,
                                    [UpdateDate] datetime DEFAULT (datetime('now'))
                                    );
                                ");

                                // 建立預設管理員帳號 (admin/admin)
                                var defaultPassword = _encryptHelper.SHAEncrypt("admin", ENCRYPT_SALT);
                                conn.Execute($@"
                                    INSERT INTO [Administrators] 
                                        ([TypeId], [Name], [Account], [Password], [UpdateUser], [UpdateDate])
                                    VALUES 
                                        (1, '系統管理員', 'admin20', '{defaultPassword}', 'System', datetime('now'))
                                ");
                            }

                            // 更新版本號
                            conn.Execute(@"
                                UPDATE [DatabaseVersion]
                                SET [Version] = 2,
                                    [UpdateDate] = datetime('now')
                            ");

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}