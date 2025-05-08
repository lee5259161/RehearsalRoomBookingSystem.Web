using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.MappingProfile;
using Serilog;

namespace RehearsalRoomBookingSystem.Service.Implements
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IServiceMapProfile _serviceMapProfile;

        public MemberService(IMemberRepository memberRepository, IServiceMapProfile serviceMapProfile)
        {
            _memberRepository = memberRepository;
            _serviceMapProfile = serviceMapProfile;
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
            return _memberRepository.GetTotalCount();
        }

        /// <summary>
        /// 分頁查詢會員資料
        /// </summary>
        /// <param name="pageNumber">頁碼，從1開始</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>指定頁碼的會員資料</returns>
        public IEnumerable<MemberDTO> GetPagedCollection(int pageNumber, int pageSize)
        {
            var entities = _memberRepository.GetPagedCollection(pageNumber, pageSize);
            return entities.Select(entity => _serviceMapProfile.MapToMemberDTO(entity));
        }

        /// <summary>
        /// 扣除會員練團卡時數
        /// </summary>
        /// <param name="memberId">會員Id</param>
        /// <param name="hours">要扣除的小時數</param>
        /// <returns>處理結果</returns>
        public UseCardTimeResultDTO UseCardTime(int memberId, int hours)
        {
            try
            {
                // 1. 驗證輸入參數
                if (hours <= 0)
                {
                    Log.Warning("扣除時數必須大於0。MemberId: {MemberId}, Hours: {Hours}", memberId, hours);
                    return new UseCardTimeResultDTO
                    {
                        Success = false,
                        Message = "扣除時數必須大於0",
                        RemainingHours = 0
                    };
                }

                // 2. 呼叫 Repository 層的方法執行扣除時數
                var resultEntity = _memberRepository.UseCardTime(memberId, hours);

                if (!resultEntity.Success)
                {
                    Log.Warning("扣除會員練團卡時數失敗。MemberId: {MemberId}, Hours: {Hours}, Message: {Message}", 
                        memberId, hours, resultEntity.Message);
                }

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
                Log.Error(ex, "扣除會員練團卡時數服務發生錯誤。MemberId: {MemberId}, Hours: {Hours}", memberId, hours);
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
        /// <param name="memberId">會員Id</param>
        /// <returns>處理結果</returns>
        public BuyCardTimeResultDTO BuyCardTime(int memberId)
        {
            try
            {
                // 1. 驗證輸入參數
                if (memberId <= 0)
                {
                    Log.Warning("無效的會員Id。MemberId: {MemberId}", memberId);
                    return new BuyCardTimeResultDTO
                    {
                        Success = false,
                        Message = "無效的會員Id",
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
                Log.Error(ex, "購買會員練團卡時數服務發生錯誤。MemberId: {MemberId}", memberId);
                return new BuyCardTimeResultDTO
                {
                    Success = false,
                    Message = "處理過程發生錯誤",
                    RemainingHours = 0
                };
            }
        }

        public IEnumerable<MemberDTO> SearchByPhone(string phone, int pageNumber, int pageSize)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    Log.Warning("搜尋會員時電話號碼為空");
                    return Enumerable.Empty<MemberDTO>();
                }

                var entities = _memberRepository.SearchByPhone(phone, pageNumber, pageSize);
                return _serviceMapProfile.MapToMemberDTOs(entities);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "搜尋會員時發生錯誤。Phone: {Phone}", phone);
                return Enumerable.Empty<MemberDTO>();
            }
        }

        public int GetTotalCountFromSearchByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                Log.Warning("計算依電話搜尋會員的數量總數時電話號碼為空");
                return 0;
            }

            return _memberRepository.GetTotalCountFromSearchByPhone(phone);
        }

        public MemberDTO GetById(int memberId)
        {
            try
            {
                if (memberId <= 0)
                {
                    Log.Warning("無效的會員Id。MemberId: {MemberId}", memberId);
                    return null;
                }

                var entity = _memberRepository.GetById(memberId);
                if (entity == null)
                {
                    Log.Warning("找不到指定的會員。MemberId: {MemberId}", memberId);
                    return null;
                }

                return _serviceMapProfile.MapToMemberDTO(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "取得會員資料時發生錯誤。MemberId: {MemberId}", memberId);
                return null;
            }
        }

        public bool UpdateMemberData(MemberDTO memberDTO)
        {
            try
            {
                if (memberDTO == null)
                {
                    Log.Warning("會員資料不可為空");
                    return false;
                }

                if (memberDTO.MemberId <= 0)
                {
                    Log.Warning("無效的會員Id。MemberId: {MemberId}", memberDTO.MemberId);
                    return false;
                }

                var entity = _serviceMapProfile.MapToMemberEntity(memberDTO);
                return _memberRepository.UpdateMemberData(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "更新會員資料時發生錯誤。MemberId: {MemberId}", memberDTO?.MemberId);
                return false;
            }
        }

        public bool IsPhoneExist(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    Log.Warning("檢查電話是否存在時，電話號碼為空");
                    return false;
                }

                return _memberRepository.IsPhoneExist(phone);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "檢查電話是否存在時發生錯誤。Phone: {Phone}", phone);
                return false;
            }
        }

        public bool CreateMember(MemberDTO memberDTO)
        {
            try
            {
                if (memberDTO == null)
                {
                    Log.Warning("創建會員時，會員資料為空");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(memberDTO.Name))
                {
                    Log.Warning("創建會員時，姓名為空");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(memberDTO.Phone))
                {
                    Log.Warning("創建會員時，電話為空");
                    return false;
                }

                // 檢查電話是否已存在
                if (IsPhoneExist(memberDTO.Phone))
                {
                    Log.Warning("創建會員時，電話已存在。Phone: {Phone}", memberDTO.Phone);
                    return false;
                }

                var entity = _serviceMapProfile.MapToMemberEntity(memberDTO);
                entity.Card_Available_Hours = 0;
                return _memberRepository.CreateMember(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "創建會員時發生錯誤");
                return false;
            }
        }
    }
}
