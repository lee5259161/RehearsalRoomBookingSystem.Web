using System.ComponentModel.DataAnnotations;

namespace RehearsalRoomBookingSystem.Web.Models.Parameter
{
    /// <summary>
    /// class UseCardTimeParameter
    /// </summary>
    public class UseCardTimeParameter
    {
        /// <summary>
        /// 會員Id
        /// </summary>
        [Required(ErrorMessage = "會員Id為必填")]
        public int MemberId { get; set; }

        /// <summary>
        /// 使用時數
        /// </summary>
        [Required(ErrorMessage = "使用時數為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "使用時數必須大於0")]
        public int UseHours { get; set; }
    }
}