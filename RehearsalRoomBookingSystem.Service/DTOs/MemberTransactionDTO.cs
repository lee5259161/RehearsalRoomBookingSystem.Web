using System;

namespace RehearsalRoomBookingSystem.Service.DTOs
{
    public class MemberTransactionDTO
    {
        public MemberTransactionDTO()
        {
            CreateUser = string.Empty;
            CreateDate = DateTime.Now;
            RecoverUser = string.Empty;
        }

        public int TransactionId { get; set; }
        public int MemberId { get; set; }
        public int TypeId { get; set; }
        public int TransactionHours { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int? RecoveryTransactionId { get; set; }
        public bool IsRecovered { get; set; }
        public string RecoverUser { get; set; }
        public DateTime? RecoverDate { get; set; }
    }
}