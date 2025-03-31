namespace RehearsalRoomBookingSystem.Web.Models.APIResult
{
    public class BuyCardTimeResult
    {
        public bool Success { get; set; }
        public decimal RemainingHours { get; set; }
        public string Message { get; set; }
    }
}
