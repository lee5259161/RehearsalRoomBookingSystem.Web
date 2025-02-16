namespace RehearsalRoomBookingSystem.Web.Models.APIResult
{
    public class UseCardTimeResult
    {
        /// <summary>
        /// 執行結果是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 剩餘時數
        /// </summary>
        public int RemainingHours { get; set; }

        /// <summary>
        /// 執行結果訊息
        /// </summary>
        public string Message { get; set; }
    }
}
