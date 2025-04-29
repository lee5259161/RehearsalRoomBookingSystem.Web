using Microsoft.Extensions.Options;
using RehearsalRoomBookingSystem.Common.Option;
using Microsoft.Data.Sqlite;
using Dapper;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Repository.Entities.ResultEntity;
using RehearsalRoomBookingSystem.Common.Interface;
using Serilog;

namespace RehearsalRoomBookingSystem.Repository.Implements
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _databasePath;
        private readonly string _connectionString;
        private readonly IUserContextHelper _userContextHelper;

        public MemberRepository(IOptions<DatabaseSettings> options, IUserContextHelper userContextHelper)
        {
            _databasePath = options.Value.SqlitePath;
            _connectionString = options.Value.ConnectionString;
            _userContextHelper = userContextHelper;
        }

        /// <summary>
        /// 取得會員總數
        /// </summary>
        /// <returns>會員總數</returns>
        public int GetTotalCount()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = "SELECT COUNT([MemberId]) FROM [Members]";
                var result = connection.QueryFirstOrDefault<int>(query);
                return result;
            }
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns>所有會員資料</returns>
        public IEnumerable<MemberEntity> GetCollection()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT m.[MemberId], m.[Name], m.[Phone], 
                                     m.[Card_Available_Hours], m.[Memo], 
                                     a.[Name] as [UpdateUser], m.[UpdateDate] 
                              FROM [Members] m
                              LEFT JOIN [Administrators] a ON m.[UpdateUser] = a.[Account]";
                var result = connection.Query<MemberEntity>(query);
                return result;
            }
        }

        public IEnumerable<MemberEntity> GetPagedCollection(int pageNumber, int pageSize)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT m.[MemberId], m.[Name], m.[Phone], 
                                     m.[Card_Available_Hours], m.[Memo], 
                                     a.[Name] as [UpdateUser], m.[UpdateDate] 
                              FROM [Members] m
                              LEFT JOIN [Administrators] a ON m.[UpdateUser] = a.[Account]
                              ORDER BY m.[MemberId] DESC
                              LIMIT @PageSize OFFSET @Offset";

                var offset = (pageNumber - 1) * pageSize;
                var result = connection.Query<MemberEntity>(query, new { PageSize = pageSize, Offset = offset });
                return result;
            }
        }

        /// <summary>
        /// 依照會員Id取得會員資料
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>會員資料</returns>
        public MemberEntity GetById(int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT [MemberId], [Name], [Phone], 
                               [Card_Available_Hours], [Memo], 
                               [UpdateUser], [UpdateDate] 
                        FROM [Members] 
                        WHERE [MemberId] = @MemberId";

                var result = connection.QueryFirstOrDefault<MemberEntity>(
                    query,
                    new { MemberId = memberId }
                );

                return result;
            }
        }

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <param name="hours">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        public CardTimeResultEntity UseCardTime(int memberId, int hours)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                // 開啟連線
                connection.Open();

                // 使用交易確保資料一致性
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. 檢查會員是否存在及其剩餘時數
                        string checkQuery = @"SELECT [Card_Available_Hours] 
                                        FROM [Members] 
                                        WHERE [MemberId] = @MemberId";

                        var currentHours = connection.QueryFirstOrDefault<int?>(
                            checkQuery,
                            new { MemberId = memberId },
                            transaction
                        );

                        // 如果找不到會員
                        if (currentHours == null)
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "找不到會員資料",
                                RemainingHours = 0
                            };
                        }

                        // 確認剩餘時數是否足夠
                        if (currentHours < hours)
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "剩餘時數不足",
                                RemainingHours = (int)currentHours
                            };
                        }

                        // 2. 更新會員剩餘時數
                        string updateQuery = @"UPDATE [Members] 
                                         SET [Card_Available_Hours] = @NewHours,
                                             [UpdateDate] = @UpdateDate,
                                             [UpdateUser] = @UpdateUser
                                         WHERE [MemberId] = @MemberId";

                        int newHours = (int)currentHours - hours;

                        var updateResult = connection.Execute(
                            updateQuery,
                            new
                            {
                                MemberId = memberId,
                                NewHours = newHours,
                                UpdateDate = DateTime.Now,
                                UpdateUser = _userContextHelper.GetCurrentUserAccount()
                            },
                            transaction
                        );

                        // 確認更新成功
                        if (updateResult != 1)
                        {
                            transaction.Rollback();
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "更新資料失敗",
                                RemainingHours = (int)currentHours
                            };
                        }

                        // 新增交易紀錄
                        string insertTransactionQuery = @"
                            INSERT INTO [MemberTransactions] 
                            ([MemberId], [TypeId], [TransactionHours], [CreateUser], [CreateDate])
                            VALUES (@MemberId, @TypeId, @TransactionHours, @CreateUser, @CreateDate)";

                        var transactionResult = connection.Execute(
                            insertTransactionQuery,
                            new
                            {
                                MemberId = memberId,
                                TypeId = 2,
                                TransactionHours = hours,
                                CreateDate = DateTime.Now,
                                CreateUser = _userContextHelper.GetCurrentUserAccount()
                            },
                            transaction
                        );

                        if (transactionResult != 1)
                        {
                            transaction.Rollback();
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "新增交易紀錄失敗",
                                RemainingHours = (int)currentHours
                            };
                        }

                        // 提交交易
                        transaction.Commit();

                        return new CardTimeResultEntity
                        {
                            Success = true,
                            Message = "扣除時數成功",
                            RemainingHours = newHours
                        };
                    }
                    catch (Exception ex)
                    {
                        // 發生錯誤時回滾交易
                        transaction.Rollback();
                        Log.Error(ex, "扣除會員練團卡時數時發生錯誤。MemberId: {MemberId}, Hours: {Hours}", memberId, hours);
                        return new CardTimeResultEntity
                        {
                            Success = false,
                            Message = "處理過程發生錯誤",
                            RemainingHours = 0
                        };
                    }
                }
            }
        }

        /// <summary>
        /// 增加會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        public BuyCardTimeResultEntity BuyCardTime(int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. 檢查會員是否存在並取得目前時數
                        var sql = @"SELECT Card_Available_Hours 
                              FROM Members 
                              WHERE MemberId = @MemberId";

                        var currentHours = connection.QueryFirstOrDefault<decimal?>(
                            sql,
                            new { MemberId = memberId },
                            transaction
                        );

                        if (!currentHours.HasValue)
                        {
                            return new BuyCardTimeResultEntity
                            {
                                Success = false,
                                Message = "找不到會員資料",
                                RemainingHours = 0
                            };
                        }

                        // 2. 計算新的時數
                        var newHours = currentHours.Value + 10;

                        // 3. 更新會員時數
                        var updateSql = @"
                        UPDATE Members 
                        SET Card_Available_Hours = @NewHours,
                            UpdateDate = @UpdateDate,
                            UpdateUser = @UpdateUser
                        WHERE MemberId = @MemberId";

                        var parameters = new
                        {
                            MemberId = memberId,
                            NewHours = newHours,
                            UpdateDate = DateTime.Now,
                            UpdateUser = _userContextHelper.GetCurrentUserAccount()
                        };

                        var affectedRows = connection.Execute(updateSql, parameters, transaction);

                        if (affectedRows <= 0)
                        {
                            transaction.Rollback();
                            return new BuyCardTimeResultEntity
                            {
                                Success = false,
                                Message = "更新時數失敗",
                                RemainingHours = currentHours.Value
                            };
                        }

                        // 4. 新增交易紀錄
                        string insertTransactionQuery = @"
                            INSERT INTO [MemberTransactions] 
                            ([MemberId], [TypeId], [TransactionHours], [CreateUser], [CreateDate])
                            VALUES (@MemberId, @TypeId, @TransactionHours, @CreateUser, @CreateDate)";

                        var transactionResult = connection.Execute(
                            insertTransactionQuery,
                            new
                            {
                                MemberId = memberId,
                                TypeId = 1,
                                TransactionHours = 10,
                                CreateDate = DateTime.Now,
                                CreateUser = _userContextHelper.GetCurrentUserAccount()
                            },
                            transaction
                        );

                        if (transactionResult != 1)
                        {
                            transaction.Rollback();
                            return new BuyCardTimeResultEntity
                            {
                                Success = false,
                                Message = "新增交易紀錄失敗",
                                RemainingHours = (int)currentHours
                            };
                        }

                        // 提交交易
                        transaction.Commit();

                        return new BuyCardTimeResultEntity
                        {
                            Success = true,
                            Message = "購買成功",
                            RemainingHours = newHours
                        };
                    }
                    catch (Exception ex)
                    {
                        // 發生異常時回滾交易
                        transaction.Rollback();
                        Log.Error(ex, "購買會員練團卡時數時發生錯誤。MemberId: {MemberId}", memberId);
                        throw;
                    }
                }
            }
        }

        public IEnumerable<MemberEntity> SearchByPhone(string phone, int pageNumber, int pageSize)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT m.[MemberId], m.[Name], m.[Phone], 
                                     m.[Card_Available_Hours], m.[Memo], 
                                     a.[Name] as [UpdateUser], m.[UpdateDate] 
                              FROM [Members] m
                              LEFT JOIN [Administrators] a ON m.[UpdateUser] = a.[Account]
                              WHERE m.[Phone] LIKE @Phone
                              ORDER BY m.[MemberId] DESC
                              LIMIT @PageSize OFFSET @Offset";

                var offset = (pageNumber - 1) * pageSize;

                var result = connection.Query<MemberEntity>(
                    query,
                    new { PageSize = pageSize, Offset = offset, Phone = $"%{phone}%" }
                );
                return result;
            }
        }

        public int GetTotalCountFromSearchByPhone(string phone)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT COUNT([MemberId]) 
                                        FROM [Members] 
                                        WHERE [Phone] LIKE @Phone";

                var result = connection.QueryFirstOrDefault<int>(
                    query,
                    new {Phone = $"%{phone}%" }
                );
                return result;
            }
        }
    }
}
