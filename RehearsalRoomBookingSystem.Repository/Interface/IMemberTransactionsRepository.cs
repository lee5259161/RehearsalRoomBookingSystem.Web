using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Repository.Entities.ResultEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Interface
{
    public interface IMemberTransactionsRepository
    {
        /// <summary>
        /// 取得會員交易紀錄
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>交易紀錄清單</returns>
        IEnumerable<MemberTransactionEntity> GetMemberTransactions(int memberId);

        /// <summary>
        /// 取得會員交易紀錄總筆數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>交易紀錄總筆數</returns>
        int GetMemberTransactionsCount(int memberId);

        /// <summary>
        /// 取得會員交易紀錄(分頁)
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>交易紀錄清單</returns>
        IEnumerable<MemberTransactionEntity> GetPagedMemberTransactions(int memberId, int pageNumber, int pageSize);

        /// <summary>
        /// 回復交易紀錄
        /// </summary>
        /// <param name="transactionId">交易Id</param>
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        CardTimeResultEntity RecoverTransaction(int transactionId, int memberId);
    }
}
