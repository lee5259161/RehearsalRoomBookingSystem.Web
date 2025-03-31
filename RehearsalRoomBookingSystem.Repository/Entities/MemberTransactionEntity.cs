using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RehearsalRoomBookingSystem.Repository.Entities
{
    public class MemberTransactionEntity
    {
        public MemberTransactionEntity()
        {
            CreateUser = string.Empty;
            CreateDate = DateTime.Now;
        }

        public int TransactionID { get; set; }
        public int MemberID { get; set; }
        public int TypeID { get; set; }
        public int TransactionHours { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
