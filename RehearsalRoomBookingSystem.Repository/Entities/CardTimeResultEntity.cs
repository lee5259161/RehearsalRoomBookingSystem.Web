using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Entities
{
    public class CardTimeResultEntity
    {
        /// <summary>
        /// 執行結果是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 執行結果訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 剩餘小時數
        /// </summary>
        public int RemainingHours { get; set; }
    }
}
