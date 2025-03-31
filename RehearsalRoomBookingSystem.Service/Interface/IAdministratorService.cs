using RehearsalRoomBookingSystem.Service.DTOs;

namespace RehearsalRoomBookingSystem.Service.Interface
{
    public interface IAdministratorService
    {
        /// <summary>
        /// 驗證管理員登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns>管理員資料，若登入失敗則返回 null</returns>
        AdministratorDTO ValidateLogin(string account, string password);

        /// <summary>
        /// 根據帳號取得管理員資料
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns>管理員資料</returns>
        AdministratorDTO GetByAccount(string account);
    }
}