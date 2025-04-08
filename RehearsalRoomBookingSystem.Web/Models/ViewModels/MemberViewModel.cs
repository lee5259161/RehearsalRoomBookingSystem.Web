using System.ComponentModel.DataAnnotations;
using RehearsalRoomBookingSystem.Web.Models.DataModel;
using X.PagedList;

namespace RehearsalRoomBookingSystem.Web.Models.ViewModels
{
    public class MemberViewModel
    {
        public IPagedList<Member> Members { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
    }
}