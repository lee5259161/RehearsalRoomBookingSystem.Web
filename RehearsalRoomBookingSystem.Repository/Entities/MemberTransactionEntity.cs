using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Entities
{
    public class MemberTransactionEntity
    {
        public int MemberID { get; set; }
        public int TransactionHours { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
