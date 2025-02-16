using System.ComponentModel.DataAnnotations;

namespace RehearsalRoomBookingSystem.Web.Models.Parameter
{
    /// <summary>
    /// class UpdatePasswordParameter
    /// </summary>
    public class MemberCreateParameter
    {
        /// <summary>Initializes a new instance of the <see cref="MemberCreateParameter" /> class.</summary>
        public MemberCreateParameter()
        {
            MemberId = "";
            Name = "";
            Email = "";
            Password = "";
            CreateUser = "";
        }

        /// <summary>
        /// 會員帳號
        /// </summary>
        [Required]
        public string MemberId { get; set; }

        /// <summary>
        /// 會員名稱
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 會員Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }
    }
}