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
        /// 分頁查詢會員資料
        /// </summary>
        /// <param name="pageNumber">頁碼，從1開始</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>指定頁碼的會員資料</returns>
        IEnumerable<MemberDTO> GetPagedCollection(int pageNumber, int pageSize);

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

        /// <summary>
        /// 依電話搜尋會員
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <param name="pageNumber">頁碼，從1開始</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>符合的會員資料清單</returns>
        IEnumerable<MemberDTO> SearchByPhone(string phone, int pageNumber, int pageSize);

        /// <summary>
        /// 取得依電話搜尋會員的會員總數
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <returns>會員總數</returns>
        int GetTotalCountFromSearchByPhone(string phone);

        /// <summary>
        /// 依照會員Id取得會員資料
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>會員資料</returns>
        MemberDTO GetById(int memberId);

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberDTO">要更新的會員資料</param>
        /// <returns>更新是否成功</returns>
        bool UpdateMemberData(MemberDTO memberDTO);

        /// <summary>
        /// 檢查電話號碼是否已存在
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <returns>是否已存在</returns>
        bool IsPhoneExist(string phone);

        /// <summary>
        /// 建立新會員
        /// </summary>
        /// <param name="memberDTO">會員資料</param>
        /// <returns>是否建立成功</returns>
        bool CreateMember(MemberDTO memberDTO);
    }
}
