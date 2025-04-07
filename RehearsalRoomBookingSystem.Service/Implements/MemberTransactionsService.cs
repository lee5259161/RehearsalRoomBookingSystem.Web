using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.MappingProfile;
using Serilog;

namespace RehearsalRoomBookingSystem.Service.Implements
{
    public class MemberTransactionsService : IMemberTransactionsService
    {
        private readonly IMemberTransactionsRepository _memberTransactionsRepository;
        private readonly IServiceMapProfile _serviceMapProfile;

        public MemberTransactionsService(IMemberTransactionsRepository memberTransactionsRepository,
                           IServiceMapProfile serviceMapProfile)
        {
            _memberTransactionsRepository = memberTransactionsRepository;
            _serviceMapProfile = serviceMapProfile;
        }

        public IEnumerable<MemberTransactionDTO> GetMemberTransactions(int memberId)
        {
            try
            {
                var result = _memberTransactionsRepository.GetMemberTransactions(memberId);
                var transactions = _serviceMapProfile.MapToMemberTransactionDTOs(result);

                return transactions;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "取得會員交易記錄時發生錯誤。MemberId: {MemberId}", memberId);
                return Enumerable.Empty<MemberTransactionDTO>();
            }
        }

        public RecoverTransactionResultDTO RecoverTransaction(int transactionId, int memberId)
        {
            try
            {
                var resultEntity = _memberTransactionsRepository.RecoverTransaction(transactionId, memberId);

                return new RecoverTransactionResultDTO
                {
                    Success = resultEntity.Success,
                    Message = resultEntity.Message,
                    RemainingHours = resultEntity.RemainingHours
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "回復交易記錄時發生錯誤。TransactionId: {TransactionId}, MemberId: {MemberId}",
                    transactionId, memberId);
                return new RecoverTransactionResultDTO
                {
                    Success = false,
                    Message = "回復過程發生錯誤",
                    RemainingHours = 0
                };
            }
        }
    }
}
