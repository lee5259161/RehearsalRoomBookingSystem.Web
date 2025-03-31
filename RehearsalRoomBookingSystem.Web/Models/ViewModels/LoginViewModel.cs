using System.ComponentModel.DataAnnotations;

namespace RehearsalRoomBookingSystem.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}