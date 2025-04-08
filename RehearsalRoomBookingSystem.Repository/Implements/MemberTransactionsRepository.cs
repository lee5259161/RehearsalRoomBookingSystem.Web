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
    public class MemberTransactionsRepository : IMemberTransactionsRepository
    {
        private readonly string _databasePath;
        private readonly string _connectionString;
        private readonly IUserContextHelper _userContextHelper;

        public MemberTransactionsRepository(IOptions<DatabaseSettings> options, IUserContextHelper userContextHelper)
        {
            _databasePath = options.Value.SqlitePath;
            _connectionString = options.Value.ConnectionString;
            _userContextHelper = userContextHelper;
        }

        public IEnumerable<MemberTransactionEntity> GetMemberTransactions(int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        mt.TransactionId,
                        mt.MemberId,
                        mt.TypeId,
                        mt.TransactionHours,
                        a.Name as CreateUser,
                        mt.CreateDate,
                        mt.RecoveryTransactionId,
                        mt.IsRecovered,
                        mt.RecoverUser,
                        mt.RecoverDate
                    FROM MemberTransactions mt
                    LEFT JOIN Administrators a ON mt.CreateUser = a.Account
                    WHERE mt.MemberId = @MemberId
                    ORDER BY mt.CreateDate DESC";

                return connection.Query<MemberTransactionEntity>(query, new { MemberId = memberId });
            }
        }

        public int GetMemberTransactionsCount(int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM MemberTransactions 
                    WHERE MemberId = @MemberId";

                return connection.ExecuteScalar<int>(query, new { MemberId = memberId });
            }
        }

        public IEnumerable<MemberTransactionEntity> GetPagedMemberTransactions(int memberId, int pageNumber, int pageSize)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        mt.TransactionId,
                        mt.MemberId,
                        mt.TypeId,
                        mt.TransactionHours,
                        a.Name as CreateUser,
                        mt.CreateDate,
                        mt.RecoveryTransactionId,
                        mt.IsRecovered,
                        mt.RecoverUser,
                        mt.RecoverDate
                    FROM MemberTransactions mt
                    LEFT JOIN Administrators a ON mt.CreateUser = a.Account
                    WHERE mt.MemberId = @MemberId
                    ORDER BY mt.CreateDate DESC
                    LIMIT @PageSize OFFSET @Offset";

                var offset = (pageNumber - 1) * pageSize;
                return connection.Query<MemberTransactionEntity>(
                    query, 
                    new { 
                        MemberId = memberId,
                        PageSize = pageSize,
                        Offset = offset
                    }
                );
            }
        }

        public CardTimeResultEntity RecoverTransaction(int transactionId, int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. 取得要回復的交易資料
                        string getTransactionQuery = @"
                            SELECT TypeId, TransactionHours 
                            FROM MemberTransactions 
                            WHERE TransactionId = @TransactionId 
                            AND MemberId = @MemberId";

                        var transactionData = connection.QueryFirstOrDefault<MemberTransactionEntity>(
                            getTransactionQuery,
                            new { TransactionId = transactionId, MemberId = memberId },
                            transaction
                        );

                        if (transactionData == null)
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "找不到指定的交易紀錄",
                                RemainingHours = 0
                            };
                        }

                        // 2. 計算回復時數
                        int hoursToRecover = transactionData.TransactionHours;
                        if (transactionData.TypeId == 1) // 購買
                        {
                            hoursToRecover = -hoursToRecover;
                        }
                        else if (transactionData.TypeId == 2) // 使用
                        {
                            // 不需要改變正負號
                        }
                        else
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "無效的交易類型",
                                RemainingHours = 0
                            };
                        }

                        // 3. 取得當前時數
                        string getCurrentHoursQuery = @"
                            SELECT Card_Available_Hours 
                            FROM Members 
                            WHERE MemberId = @MemberId";

                        var currentHours = connection.QueryFirstOrDefault<int?>(
                            getCurrentHoursQuery,
                            new { MemberId = memberId },
                            transaction
                        );

                        if (!currentHours.HasValue)
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "找不到會員資料",
                                RemainingHours = 0
                            };
                        }

                        // 4. 更新時數
                        int newHours = currentHours.Value + hoursToRecover;

                        if (newHours < 0)
                        {
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "回復後時數會小於0，無法進行回復",
                                RemainingHours = currentHours.Value
                            };
                        }

                        string updateMemberQuery = @"
                            UPDATE Members 
                            SET Card_Available_Hours = @NewHours 
                            WHERE MemberId = @MemberId";

                        var updateResult = connection.Execute(
                            updateMemberQuery,
                            new { NewHours = newHours, MemberId = memberId },
                            transaction
                        );

                        if (updateResult != 1)
                        {
                            transaction.Rollback();
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "更新時數失敗",
                                RemainingHours = currentHours.Value
                            };
                        }

                        // 5. 把原始交易標記為已回復
                        string updateTransactionQuery = @"
                            UPDATE MemberTransactions 
                            SET RecoverDate = @RecoverDate,
                                RecoverUser = @RecoverUser
                            WHERE TransactionId = @TransactionId";

                        var markResult = connection.Execute(
                            updateTransactionQuery,
                            new
                            {
                                TransactionId = transactionId,
                                RecoverDate = DateTime.Now,
                                RecoverUser = _userContextHelper.GetCurrentUserAccount()
                            },
                            transaction
                        );

                        if (markResult != 1)
                        {
                            transaction.Rollback();
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "標記交易回復狀態失敗",
                                RemainingHours = currentHours.Value
                            };
                        }

                        // 6. 新增回復交易紀錄
                        string insertRecoveryQuery = @"
                            INSERT INTO MemberTransactions 
                            (MemberId, TypeId, TransactionHours, CreateUser, CreateDate, RecoveryTransactionId)
                            VALUES 
                            (@MemberId, @TypeId, @Hours, @CreateUser, @CreateDate, @RecoveryTransactionId)";

                        var insertResult = connection.Execute(
                            insertRecoveryQuery,
                            new
                            {
                                MemberId = memberId,
                                TypeId = transactionData.TypeId == 1 ? 3 : 4, // 3 for recovering purchase, 4 for recovering usage
                                Hours = hoursToRecover,
                                CreateUser = _userContextHelper.GetCurrentUserAccount(),
                                CreateDate = DateTime.Now,
                                RecoveryTransactionId = transactionId
                            },
                            transaction
                        );

                        if (insertResult != 1)
                        {
                            transaction.Rollback();
                            return new CardTimeResultEntity
                            {
                                Success = false,
                                Message = "新增回復交易紀錄失敗",
                                RemainingHours = currentHours.Value
                            };
                        }

                        transaction.Commit();

                        return new CardTimeResultEntity
                        {
                            Success = true,
                            Message = "回復成功",
                            RemainingHours = newHours
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex, "回復交易記錄時發生錯誤。TransactionId: {TransactionId}, MemberId: {MemberId}",
                            transactionId, memberId);
                        return new CardTimeResultEntity
                        {
                            Success = false,
                            Message = "回復過程發生錯誤",
                            RemainingHours = 0
                        };
                    }
                }
            }
        }
    }
}
