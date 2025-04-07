namespace RehearsalRoomBookingSystem.Service.DTOs
{
    public class AdministratorDTO
    {
        public AdministratorDTO()
        {
            Name = string.Empty;
            Account = string.Empty;
            UpdateUser = string.Empty;
            UpdateDate = DateTime.Now;
        }

        public int AdminId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}