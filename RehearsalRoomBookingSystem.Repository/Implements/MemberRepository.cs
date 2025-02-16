using Microsoft.Extensions.Options;
using RehearsalRoomBookingSystem.Common.Option;
using Microsoft.Data.Sqlite;
using Dapper;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Repository.Entities;

namespace RehearsalRoomBookingSystem.Repository.Implements
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _databasePath;
        private readonly string _connectionString;

        public MemberRepository(IOptions<DatabaseSettings> options)
        {
            _databasePath = options.Value.SqlitePath;
            _connectionString = options.Value.ConnectionString;
        }

        /// <summary>
        /// 取得會員總數
        /// </summary>
        /// <returns>會員總數</returns>
        public int GetTotalCount()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = "SELECT COUNT([MemberID]) FROM [Members]";
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
                string query = @"SELECT [MemberID], [Name], [Phone], 
                                     [Card_Available_Hours], [Memo], 
                                     [UpdateUser], [UpdateDate] FROM [Members]";
                var result = connection.Query<MemberEntity>(query);
                return result;
            }
        }

        /// <summary>
        /// 依照會員ID取得會員資料
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <returns>會員資料</returns>
        public MemberEntity GetById(int memberId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string query = @"SELECT [MemberID], [Name], [Phone], 
                               [Card_Available_Hours], [Memo], 
                               [UpdateUser], [UpdateDate] 
                        FROM [Members] 
                        WHERE [MemberID] = @MemberId";

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
        /// <param name="memberId">會員ID</param>
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
                                        WHERE [MemberID] = @MemberId";

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
                                         WHERE [MemberID] = @MemberId";

                        int newHours = (int)currentHours - hours;

                        var updateResult = connection.Execute(
                            updateQuery,
                            new
                            {
                                MemberId = memberId,
                                NewHours = newHours,
                                UpdateDate = DateTime.Now,
                                UpdateUser = "System" // 這裡可能需要傳入實際的使用者
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
    }
}
