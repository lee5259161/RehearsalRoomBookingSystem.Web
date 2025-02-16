using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Repository.Entities
{
    /// <summary>
    /// Represents a MemberEntity in the rehearsal room booking system.
    /// </summary>
    public class MemberEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberEntity"/> class.
        /// </summary>
        public MemberEntity()
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
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the member's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the member's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the number of available hours on the member's card.
        /// </summary>
        public int Card_Available_Hours { get; set; }

        /// <summary>
        /// Gets or sets any additional memo for the member.
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated the member's information.
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the member's information was last updated.
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
