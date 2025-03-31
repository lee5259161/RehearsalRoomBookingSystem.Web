using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.MappingProfile;

namespace RehearsalRoomBookingSystem.Service.Implements
{
    public class MemberService : IMemberService
    {
        //注入MemberRepository
        private readonly IMemberRepository _memberRepository;
        private readonly IServiceMapProfile _serviceMapProfile;

        public MemberService(IMemberRepository memberRepository,
                             IServiceMapProfile serviceMapProfile)
        {
            this._memberRepository = memberRepository;
            this._serviceMapProfile = serviceMapProfile;
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns>所有會員資料的集合</returns>
        public IEnumerable<MemberDTO> GetCollection()
        {

            //先取得會員總數，如果沒資料就直接回傳
            var memberAmount = this._memberRepository.GetTotalCount();

            if (memberAmount.Equals(0))
            {
                return Enumerable.Empty<MemberDTO>();
            }

            var result = _memberRepository.GetCollection();
            var members = _serviceMapProfile.MapToMemberDTOs(result);

            return members;
        }

        /// <summary>
        /// 取得會員總數
        /// </summary>
        /// <returns>會員總數</returns>
        public int GetTotalCount()
        {
            //先取得會員總數，如果沒資料就直接回傳
            var memberAmount = this._memberRepository.GetTotalCount();

            return memberAmount;

        }

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <param name="hours">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        public UseCardTimeResultDTO UseCardTime(int memberId, int hours)
        {
            try
            {
                // 1. 驗證輸入參數
                if (hours <= 0)
                {
                    return new UseCardTimeResultDTO
                    {
                        Success = false,
                        Message = "扣除時數必須大於0",
                        RemainingHours = 0
                    };
                }

                // 2. 呼叫 Repository 層的方法執行扣除時數
                var resultEntity = _memberRepository.UseCardTime(memberId, hours);

                // 3. 轉換 Entity 到 DTO 並回傳結果
                return new UseCardTimeResultDTO
                {
                    Success = resultEntity.Success,
                    Message = resultEntity.Message,
                    RemainingHours = resultEntity.RemainingHours
                };
            }
            catch (Exception ex)
            {
                return new UseCardTimeResultDTO
                {
                    Success = false,
                    Message = "處理過程發生錯誤",
                    RemainingHours = 0
                };
            }
        }

        /// <summary>
        /// 購買會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <returns>處理結果</returns>
        public BuyCardTimeResultDTO BuyCardTime(int memberId)
        {
            try
            {
                // 1. 驗證輸入參數
                if (memberId <= 0)
                {
                    return new BuyCardTimeResultDTO
                    {
                        Success = false,
                        Message = "無效的會員ID",
                        RemainingHours = 0
                    };
                }

                // 2. 呼叫 Repository 層的方法執行增加時數
                var resultEntity = _memberRepository.BuyCardTime(memberId);

                // 3. 轉換 Entity 到 DTO 並回傳結果
                return new BuyCardTimeResultDTO
                {
                    Success = resultEntity.Success,
                    Message = resultEntity.Message,
                    RemainingHours = resultEntity.RemainingHours
                };
            }
            catch (Exception ex)
            {
                return new BuyCardTimeResultDTO
                {
                    Success = false,
                    Message = "處理過程發生錯誤",
                    RemainingHours = 0
                };
            }
        }
    }
}
