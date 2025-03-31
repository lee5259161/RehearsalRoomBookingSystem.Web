using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Entities.ResultEntity
{
    /// <summary>
    /// 增加會員練團卡時數的結果實體
    /// </summary>
    public class BuyCardTimeResultEntity
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal RemainingHours { get; set; }
    }
}
