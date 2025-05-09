﻿using System;
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
