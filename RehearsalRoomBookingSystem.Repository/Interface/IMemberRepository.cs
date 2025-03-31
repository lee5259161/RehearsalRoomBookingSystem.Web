using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Repository.Entities.ResultEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Interface
{
    public interface IMemberRepository
    {
        /// <summary>
        /// 取得會員總數
        /// </summary>
        /// <returns>會員總數</returns>
        int GetTotalCount();

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns>所有會員資料</returns>
        IEnumerable<MemberEntity> GetCollection();

        /// <summary>
        /// 依照會員ID取得會員資料
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <returns>會員資料</returns>
        MemberEntity GetById(int memberId);

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <param name="minutes">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        CardTimeResultEntity UseCardTime(int memberId, int hours);

        /// <summary>
        /// 增加會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員ID</param>
        BuyCardTimeResultEntity BuyCardTime(int memberId);
    }
}
