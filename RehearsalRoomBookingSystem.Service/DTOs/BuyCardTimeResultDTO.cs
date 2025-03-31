using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Service.DTOs
{
    public class BuyCardTimeResultDTO
    {
        /// <summary>
        /// 處理是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 處理結果訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 剩餘時數
        /// </summary>
        public decimal RemainingHours { get; set; }
    }
}
