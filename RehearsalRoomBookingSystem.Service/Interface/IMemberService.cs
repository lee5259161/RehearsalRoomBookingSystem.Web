using RehearsalRoomBookingSystem.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Service.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 取得會員總數
        /// </summary>
        /// <returns>會員總數</returns>
        int GetTotalCount();

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns>所有會員資料的集合</returns>
        IEnumerable<MemberDTO> GetCollection();

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <param name="hours">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        UseCardTimeResultDTO UseCardTime(int memberId, int minutes);
       
        /// <summary>
        /// 購買會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        BuyCardTimeResultDTO BuyCardTime(int memberId);
    }
}
