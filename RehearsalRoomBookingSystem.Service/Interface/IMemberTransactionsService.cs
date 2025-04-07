using RehearsalRoomBookingSystem.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Service.Interface
{
    public interface IMemberTransactionsService
    {
        /// <summary>
        /// 取得會員交易紀錄
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>交易紀錄清單</returns>
        IEnumerable<MemberTransactionDTO> GetMemberTransactions(int memberId);

        /// <summary>
        /// 回復交易紀錄
        /// </summary>
        /// <param name="transactionId">交易Id</param>
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        RecoverTransactionResultDTO RecoverTransaction(int transactionId, int memberId);
    }
}
