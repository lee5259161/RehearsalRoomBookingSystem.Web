using System.ComponentModel.DataAnnotations;
using RehearsalRoomBookingSystem.Web.Models.DataModel;

namespace RehearsalRoomBookingSystem.Web.Models.ViewModels
{

    public class MemberViewModel
    {
        public List<Member> Members { get; set; }
        public int PageIndex { get; set; }
    }
}