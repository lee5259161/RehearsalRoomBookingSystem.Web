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
        /// 分頁查詢會員資料
        /// </summary>
        /// <param name="pageNumber">頁碼，從1開始</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>指定頁碼的會員資料</returns>
        IEnumerable<MemberEntity> GetPagedCollection(int pageNumber, int pageSize);

        /// <summary>
        /// 依照會員Id取得會員資料
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>會員資料</returns>
        MemberEntity GetById(int memberId);

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <param name="minutes">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        CardTimeResultEntity UseCardTime(int memberId, int hours);

        /// <summary>
        /// 增加會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        BuyCardTimeResultEntity BuyCardTime(int memberId);

        /// <summary>
        /// 依電話搜尋會員
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <returns>符合的會員資料清單</returns>
        IEnumerable<MemberEntity> SearchByPhone(string phone, int pageNumber, int pageSize);

        /// <summary>
        /// 取得依電話搜尋會員的會員總數
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <returns>會員總數</returns>
        int GetTotalCountFromSearchByPhone(string phone);

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="entity">要更新的會員資料</param>
        /// <returns>更新是否成功</returns>
        bool UpdateMemberData(MemberEntity entity);

        /// <summary>
        /// 檢查電話號碼是否已存在
        /// </summary>
        /// <param name="phone">電話號碼</param>
        /// <returns>是否已存在</returns>
        bool IsPhoneExist(string phone);

        /// <summary>
        /// 建立新會員
        /// </summary>
        /// <param name="entity">會員資料</param>
        /// <returns>是否建立成功</returns>
        bool CreateMember(MemberEntity entity);
    }
}
