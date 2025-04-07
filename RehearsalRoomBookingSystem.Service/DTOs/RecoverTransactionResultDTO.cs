namespace RehearsalRoomBookingSystem.Service.DTOs
{
    public class RecoverTransactionResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RemainingHours { get; set; }
    }
}