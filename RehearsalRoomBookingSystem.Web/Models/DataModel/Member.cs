using System.ComponentModel.DataAnnotations;

namespace RehearsalRoomBookingSystem.Web.Models.DataModel
{

    /// <summary>
    /// Represents a member in the system.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> class.
        /// </summary>
        public Member()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Memo = string.Empty;
            UpdateUser = string.Empty;
            UpdateDate = DateTime.Now;
    }

        /// <summary>
        /// Gets or sets the member ID.
        /// </summary>
        [Display(Name = "會員編號")]
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the member's name.
        /// </summary>
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the member's phone number.
        /// </summary>
        [Display(Name = "電話")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the number of available hours on the member's card.
        /// </summary>
        [Display(Name = "團卡時數")]
        public int Card_Available_Hours { get; set; }

        /// <summary>
        /// Gets or sets any additional memo for the member.
        /// </summary>
        [Display(Name = "備註")]
        public string Memo { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated the member's information.
        /// </summary>
        [Display(Name = "更新人員")]
        public string UpdateUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the member's information was last updated.
        /// </summary>
        [Display(Name = "更新日期")]
        public DateTime UpdateDate { get; set; }
    }
}

